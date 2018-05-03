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

namespace UnityDisk.StorageItems
{
    public class StorageFolde:IStorageFolder2
    {
        private IUnityContainer _container;
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

        public StorageFolde()
        {
            State = StorageItemStateEnum.Exist;
        }

        public StorageFolde(IFileStorageFolder dataContext)
        {
            DataContext = dataContext;
            State = StorageItemStateEnum.Exist;
        }
        public void Rename(string newName)
        {
            DataContext.Rename(newName);
        }

        public void Delete()
        {
            DataContext.Delete();
            State = StorageItemStateEnum.Deleted;
        }

        public void Move(IStorageProjectionFolder folder)
        {
            IStorageFolder2 toFolder = folder.Folders.FirstOrDefault(folder2 => folder2.Account.Login == Account.Login);
            if (toFolder == null)
                folder.MakeProjection();

            toFolder = folder.Folders.FirstOrDefault(folder2 => folder2.Account.Login == Account.Login);
            if (toFolder == null)
                throw new ArgumentException("Not found Account");

            DataContext.Move(toFolder.DataContext);
        }

        public void Copy(IStorageProjectionFolder folder)
        {
            IStorageFolder2 toFolder = folder.Folders.FirstOrDefault(folder2 => folder2.Account.Login == Account.Login);
            if (toFolder == null)
                folder.MakeProjection();

            toFolder = folder.Folders.FirstOrDefault(folder2 => folder2.Account.Login == Account.Login);
            if (toFolder == null)
                throw new ArgumentException("Not found Account");

            DataContext.Copy(toFolder.DataContext);
        }

        public void LoadPreviewImage()
        {
            DataContext.LoadPreviewImage();
        }

        public void LoadPublicUrl()
        {
            DataContext.LoadPublicUrl();
        }

        public void CreatePublicUrl()
        {
            DataContext.CreatePublicUrl();
        }

        public void DeletePublicUrl()
        {
            DataContext.DeletePublicUrl();
        }

        public List<IStorageItem> LoadDirectory()
        {
            IList<IFileStorageItem> items= DataContext.LoadDirectory();
            List<IStorageItem> result=new List<IStorageItem>(items.Count);
            foreach (var item in items)
            {
                IStorageItem storageItem = null;
                switch (item)
                {
                    case IFileStorageFile fileStorageFile:
                        var folder = _container.Resolve<IStorageFile>();
                        folder.DataContext = fileStorageFile;
                        storageItem = folder;
                        break;
                    case IFileStorageFolder fileStorageFolder:
                        var file = _container.Resolve<IStorageFolder2>();
                        file.DataContext = fileStorageFolder;
                        storageItem = file;
                        break;
                    default:
                        throw new ArgumentException("Unknown type");
                }
                result.Add(storageItem);
            }

            return result;
        }

        public void Upload(Windows.Storage.IStorageFile storageFile)
        {
            DataContext.Upload(storageFile);
        }

        public IStorageFolder CreateFolder(string name)
        {
            return CreateFolder2(name);
        }

        public void MakeProjection()
        {
            List<IStorageProjectionFolder> myPath = new List<IStorageProjectionFolder>();

            IStorageProjectionFolder topParent = Parent;
            while (topParent.Parent != null)
            {
                myPath.Insert(0, topParent);
                topParent = topParent.Parent;
            }

            MakeBranch(myPath);
        }

        private void MakeBranch(IList<IStorageProjectionFolder> path)
        {
            var fistItem = path.GetEnumerator();
            if(path.Count<2)return;

            var secondItem = path.GetEnumerator();
            
            bool canSecondItemGoNext= true;
            secondItem.MoveNext(); // 1

            while (canSecondItemGoNext)
            {
                fistItem.MoveNext(); // 1
                canSecondItemGoNext= secondItem.MoveNext(); // 2
                if (fistItem.Current.Folders.Count > secondItem.Current.Folders.Count)
                {
                    var needCreateFolderIn
                        = from f1 in fistItem.Current.Folders
                        from f2 in secondItem.Current.Folders
                        where f1.Account.Login != f2.Account.Login
                        select f1;
                    foreach (var forCreateFolder in needCreateFolderIn)
                    {
                       var newFolder= forCreateFolder.CreateFolder2(secondItem.Current.Name);
                        secondItem.Current.Folders.Add(newFolder);
                    }
                }
            }
        }
        public IStorageFolder2 CreateFolder2(string name)
        {
            var result = new StorageFolde
            {
                DataContext = DataContext.CreateFolder(name)
            };
            return result;
        }
    }
}
