using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Audio;
using Windows.Storage.BulkAccess;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;
using UnityDisk.FileStorages;

namespace UnityDisk.StorageItems
{
    public class StorageProjectionFolder:IStorageProjectionFolder
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public BitmapImage PreviewImage { get; set; }
        public StorageItemTypeEnum Type => StorageItemTypeEnum.Directory;
        public StorageItemStateEnum State { get; private set; }
        public IStorageProjectionFolder Parent { get; set; }
        public IList<IStorageFolder2> Folders { get; }

        public StorageProjectionFolder()
        {
            Folders=new List<IStorageFolder2>();
            State = StorageItemStateEnum.Exist;
        }
        public async Task Rename(string newName)
        {
            foreach (var folder in Folders)
            {
               await folder.Rename(newName);
            }
        }

        public async Task Delete()
        {
            foreach (var folder in Folders)
            {
                await folder.Delete();
            }
        }

        public async Task Move(IStorageProjectionFolder folder)
        {
            foreach (var item in Folders)
            {
                await item.Move(folder);
            }
        }

        public async Task<IStorageItem> Copy(IStorageProjectionFolder folder)
        {
            StorageProjectionFolder projectionFolder=new StorageProjectionFolder()
            {
                PreviewImage = PreviewImage,
                Parent = folder,
                Path = folder.Path
            };
            foreach (var item in Folders)
            {
                IFileStorageFolder newFolder = await item.Copy(folder) as IFileStorageFolder;
                if(newFolder==null)
                    throw new NullReferenceException("Did not get folder after copy");

                projectionFolder.Folders.Add(new StorageFolder(){DataContext = newFolder});
            }

            return projectionFolder;
        }

        public async Task LoadPreviewImage()
        {
            foreach (var item in Folders)
            {
                await item.LoadPreviewImage();
            }
        }

        public async Task LoadPublicUrl()
        {
            foreach (var item in Folders)
            {
                await item.LoadPublicUrl();
            }
        }

        public async Task CreatePublicUrl()
        {
            foreach (var item in Folders)
            {
                await item.CreatePublicUrl();
            }
        }

        public async Task DeletePublicUrl()
        {
            foreach (var item in Folders)
            {
              await  item.DeletePublicUrl();
            }
        }

        public async Task<List<IStorageItem>>LoadDirectory()
        {
            List<IStorageItem> result = new List<IStorageItem>();

            foreach (var item in Folders)
            {
                var directory = await item.LoadDirectory();
                foreach (var item2 in directory)
                {
                    if (item2.Type.HasFlag(StorageItemTypeEnum.Directory))
                    {
                        var folder = result.FirstOrDefault(storageItem =>
                            storageItem.Type.HasFlag(StorageItemTypeEnum.Directory) &&
                            storageItem.Name == item2.Name) as IStorageProjectionFolder;
                        if (folder == null)
                            result.Add(new StorageProjectionFolder()
                            {
                                Name = item2.Name,
                                Parent = this,
                                Folders = {(IStorageFolder2) item2},
                                Path = String.Concat("{0}\\{1}", Path, item2.Name),
                                PreviewImage = PreviewImage,
                            });
                        else
                            folder.Folders.Add((IStorageFolder2) item2);
                    }

                    result.Add(item2);
                }
            }

            return result;
        }

        public async Task<BackgroundOperation.IUploader> Upload(Windows.Storage.IStorageFile storageFile)
        {
            BasicProperties basicProperties=await storageFile.GetBasicPropertiesAsync().AsTask();
            var folders = from f1 in Folders
                where f1.Account.Size.FreelSize > basicProperties.Size
                select f1;
            IStorageFolder folder= folders.First();
            if(folder==null)
                throw new InvalidOperationException("You have not enough memory");

           return await folder.Upload(storageFile);
        }

        public async Task<IStorageFolder> CreateFolder(string name)
        {
            var newFolder=new StorageProjectionFolder()
            {
                Name = name,
                Parent = this,
                PreviewImage = PreviewImage,
                Path = String.Concat("{0}\\{1}", Path,Name)
            };
            foreach (var item in Folders)
            {
                newFolder.Folders.Add(await item.CreateFolder2(name));
            }

            return newFolder;
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
            var fistItem = path.GetEnumerator();
            if (path.Count < 2) return;

            var secondItem = path.GetEnumerator();

            bool canSecondItemGoNext = true;
            secondItem.MoveNext(); // 1

            while (canSecondItemGoNext)
            {
                fistItem.MoveNext(); // 1
                canSecondItemGoNext = secondItem.MoveNext(); // 2
                if (fistItem.Current.Folders.Count > secondItem.Current.Folders.Count)
                {
                    var needCreateFolderIn
                        = from f1 in fistItem.Current.Folders
                        from f2 in secondItem.Current.Folders
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

        public async Task Delete(IList<IStorageFolder2> folders)
        {
            var foldersForDelete = from f1 in Folders
                from f2 in folders
                where f1.Account.Login == f2.Account.Login
                select f1;

            foreach (var item in foldersForDelete)
            {
                await item.DeletePublicUrl();
            }
        }

        public async Task Rename(IList<IStorageFolder2> folders,string newName)
        {
            var foldersForDelete = from f1 in Folders
                from f2 in folders
                where f1.Account.Login == f2.Account.Login
                select f1;

            foreach (var item in foldersForDelete)
            {
                await item.Rename(newName);
            }
        }

        public async Task Move(IList<IStorageFolder2> folders, IStorageProjectionFolder folder)
        {
            var foldersForDelete = from f1 in Folders
                from f2 in folders
                where f1.Account.Login == f2.Account.Login
                select f1;

            foreach (var item in foldersForDelete)
            {
                await item.Move(folder);
            }
        }

        public async Task Copy(IList<IStorageFolder2> folders, IStorageProjectionFolder folder)
        {
            var foldersForDelete = from f1 in Folders
                from f2 in folders
                where f1.Account.Login == f2.Account.Login
                select f1;

            foreach (var item in foldersForDelete)
            {
                await item.Copy(folder);
            }
        }

        public async Task CreatePublicUrl(IList<IStorageFolder2> folders)
        {
            var foldersForDelete = from f1 in Folders
                from f2 in folders
                where f1.Account.Login == f2.Account.Login
                select f1;

            foreach (var item in foldersForDelete)
            {
                await item.CreatePublicUrl();
            }
        }

        public Task LoadPublicUrl(IList<IStorageFolder2> folders)
        {
            //var foldersForDelete = from f1 in Folders
            //    from f2 in folders
            //    where f1.Account.Login == f2.Account.Login
            //    select f1;

            //foreach (var item in foldersForDelete)
            //{
            //    item.LoadPublicUrl();
            //}
            return new Task(() => { });
        }

        public async Task DeletePublicUrl(IList<IStorageFolder2> folders)
        {
            var foldersForDelete = from f1 in Folders
                from f2 in folders
                where f1.Account.Login == f2.Account.Login
                select f1;

            foreach (var item in foldersForDelete)
            {
                await item.DeletePublicUrl();
            }
        }
    }
}
