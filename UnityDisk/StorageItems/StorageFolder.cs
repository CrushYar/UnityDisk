using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Unity;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages;
using UnityDisk.StorageItems.PreviewImageManager;

namespace UnityDisk.StorageItems
{
    public class StorageFolder:IStorageFolder2
    {
        private readonly IUnityContainer _container;
        public IFileStorageFolder DataContext { get; set; }

        public string PublicUrl => DataContext.PublicUrl;
        public IAccountProjection Account
        {
            get => DataContext.Account;
            set => DataContext.Account = value;
        }
        public DateTime CreateDate => DataContext.CreateDate;
        public string Name => DataContext.Name;
        public string Path => DataContext.Path;
        public BitmapImage PreviewImage
        {
            get => DataContext.PreviewImage;
            set => DataContext.PreviewImage = value;
        }
        public StorageItemAttributeEnum Attribute => DataContext.Attribute;
        public StorageItemStateEnum State { get; private set; }
        public IStorageProjectionFolder Parent { get;  set; }

        public StorageFolder()
        {
            State = StorageItemStateEnum.Exist;
            _container = ContainerConfiguration.GetContainer().Container;
        }

        public StorageFolder(IFileStorageFolder dataContext)
        {
            DataContext = dataContext;
            State = StorageItemStateEnum.Exist;
        }
        public async Task Rename(string newName)
        {
           await DataContext.Rename(newName);
        }

        public async Task Delete()
        {
            await DataContext.Delete();
            State = StorageItemStateEnum.Deleted;
        }

        public async Task Move(IStorageProjectionFolder folder)
        {
            IStorageFolder2 toFolder = folder.Folders.FirstOrDefault(folder2 => folder2.Account.Login == Account.Login);
            if (toFolder == null)
                await folder.MakeProjection();

            toFolder = folder.Folders.FirstOrDefault(folder2 => folder2.Account.Login == Account.Login);
            if (toFolder == null)
                throw new ArgumentException("Not found Account");

            await DataContext.Move(toFolder.DataContext);
        }

        public async Task<IStorageItem> Copy(IStorageProjectionFolder folder)
        {
            IStorageFolder2 toFolder = folder.Folders.FirstOrDefault(folder2 => folder2.Account.Login == Account.Login);
            if (toFolder == null)
                await folder.MakeProjection();

            toFolder = folder.Folders.FirstOrDefault(folder2 => folder2.Account.Login == Account.Login);
            if (toFolder == null)
                throw new ArgumentException("Not found Account");

            IFileStorageFolder storageFolder= await DataContext.Copy(toFolder.DataContext) as IFileStorageFolder;
            if (storageFolder == null)
                throw new NullReferenceException("Did not get folder after copy");

            return new StorageFolder()
            {
                DataContext = storageFolder,
                Account = Account
            };
        }

        public async Task LoadPreviewImage()
        {
            await DataContext.LoadPreviewImage();
        }

        public async Task LoadPublicUrl()
        {
            await DataContext.LoadPublicUrl();
        }

        public async Task CreatePublicUrl()
        {
            await DataContext.CreatePublicUrl();
        }

        public async Task DeletePublicUrl()
        {
            await DataContext.DeletePublicUrl();
        }

        public async Task<List<IStorageItem>> LoadDirectory()
        {
            IList<IFileStorageItem> items= await DataContext.LoadDirectory();
            List<IStorageItem> result=new List<IStorageItem>(items.Count);
            IStandardPreviewImagesRegistry imagesRegistry = _container.Resolve<IStandardPreviewImagesRegistry>();
            foreach (var item in items)
            {
                IStorageItem storageItem = null;
                switch (item)
                {
                    case IFileStorageFile fileStorageFile:
                        var file = _container.Resolve<IStorageFile>();
                        file.DataContext = fileStorageFile;
                        storageItem = file;
                        file.PreviewImage = imagesRegistry.FindPreviewImage(file.Type);
                        break;
                    case IFileStorageFolder fileStorageFolder:
                        var folder = new StorageFolder();
                        folder.DataContext = fileStorageFolder;
                        storageItem = folder;
                        folder.PreviewImage = imagesRegistry.FindPreviewImage("folder");
                        break;
                    default:
                        throw new ArgumentException("Unknown type");
                }

                result.Add(storageItem);
            }

            return result;
        }

        public async Task Upload(Windows.Storage.IStorageFile storageFile)
        {
            await DataContext.Upload(storageFile);
        }

        public async Task<IStorageFolder> CreateFolder(string name)
        {
           return await CreateFolder2(name);
        }

        public async Task MakeProjection()
        {
            List<IStorageProjectionFolder> myPath = new List<IStorageProjectionFolder>();

            IStorageProjectionFolder topParent = Parent;
            while (topParent.Parent != null)
            {
                myPath.Insert(0, topParent);
                topParent = topParent.Parent;
            }

            await MakeBranch(myPath);
        }

        private async Task MakeBranch(IList<IStorageProjectionFolder> path)
        {
            using (var fistItem = path.GetEnumerator())
            {
                if (path.Count < 2) return;

                using (var secondItem = path.GetEnumerator())
                {

                    bool canSecondItemGoNext = true;
                    secondItem.MoveNext(); // 1

                    while (canSecondItemGoNext)
                    {
                        fistItem.MoveNext(); // 1
                        canSecondItemGoNext = secondItem.MoveNext(); // 2
                        if (fistItem.Current.Folders.Count > secondItem.Current.Folders.Count)
                        {
                            var enumerator = secondItem;
                            var needCreateFolderIn
                                = from f1 in fistItem.Current.Folders
                                from f2 in enumerator.Current.Folders
                                where f1.Account.Login != f2.Account.Login
                                select f1;
                            foreach (var forCreateFolder in needCreateFolderIn)
                            {
                                var newFolder = await forCreateFolder.CreateFolder2(secondItem.Current.Name);
                                secondItem.Current.Folders.Add(newFolder);
                            }
                        }
                    }
                }
            }

        }
        public async Task<IStorageFolder2> CreateFolder2(string name)
        {
            var result = new StorageFolder
            {
                DataContext = await DataContext.CreateFolder(name)
            };
            return result;
        }
    }
}
