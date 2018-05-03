﻿using System;
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
        public StorageItemAttributeEnum Attribute => StorageItemAttributeEnum.Directory;
        public StorageItemStateEnum State { get; private set; }
        public IStorageProjectionFolder Parent { get; set; }
        public IList<IStorageFolder2> Folders { get; }

        public StorageProjectionFolder()
        {
            Folders=new List<IStorageFolder2>();
            State = StorageItemStateEnum.Exist;
        }
        public void Rename(string newName)
        {
            foreach (var folder in Folders)
            {
                folder.Rename(newName);
            }
        }

        public void Delete()
        {
            foreach (var folder in Folders)
            {
                folder.Delete();
            }
        }

        public void Move(IStorageProjectionFolder folder)
        {
            foreach (var item in Folders)
            {
                item.Move(folder);
            }
        }

        public void Copy(IStorageProjectionFolder folder)
        {
            foreach (var item in Folders)
            {
                item.Copy(folder);
            }
        }

        public void LoadPreviewImage()
        {
            foreach (var item in Folders)
            {
                item.LoadPreviewImage();
            }
        }

        public void LoadPublicUrl()
        {
            foreach (var item in Folders)
            {
                item.LoadPublicUrl();
            }
        }

        public void CreatePublicUrl()
        {
            foreach (var item in Folders)
            {
                item.CreatePublicUrl();
            }
        }

        public void DeletePublicUrl()
        {
            foreach (var item in Folders)
            {
                item.DeletePublicUrl();
            }
        }

        public List<IStorageItem> LoadDirectory()
        {
            List<IStorageItem> result = new List<IStorageItem>();

            foreach (var item in Folders)
            {
                var directory = item.LoadDirectory();
                foreach (var item2 in directory)
                {
                    if (item2.Attribute.HasFlag(StorageItemAttributeEnum.Directory))
                    {
                        var folder = result.FirstOrDefault(storageItem =>
                            storageItem.Attribute.HasFlag(StorageItemAttributeEnum.Directory) &&
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

        public async void Upload(Windows.Storage.IStorageFile storageFile)
        {
            BasicProperties basicProperties=await storageFile.GetBasicPropertiesAsync().AsTask();
            var folders = from f1 in Folders
                where f1.Account.Size.FreelSize > basicProperties.Size
                select f1;
            IStorageFolder folder= folders.First();
            if(folder==null)
                throw new InvalidOperationException("You have not enough memory");

            folder.Upload(storageFile);
        }

        public IStorageFolder CreateFolder(string name)
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
                newFolder.Folders.Add(item.CreateFolder2(name));
            }

            return newFolder;
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
                        var newFolder = forCreateFolder.CreateFolder2(secondItem.Current.Name);
                        secondItem.Current.Folders.Add(newFolder);
                    }
                }
            }
        }

        public void Delete(IList<IStorageFolder2> folders)
        {
            var foldersForDelete = from f1 in Folders
                from f2 in folders
                where f1.Account.Login == f2.Account.Login
                select f1;

            foreach (var item in foldersForDelete)
            {
                item.DeletePublicUrl();
            }
        }

        public void Rename(IList<IStorageFolder2> folders,string newName)
        {
            var foldersForDelete = from f1 in Folders
                from f2 in folders
                where f1.Account.Login == f2.Account.Login
                select f1;

            foreach (var item in foldersForDelete)
            {
                item.Rename(newName);
            }
        }

        public void Move(IList<IStorageFolder2> folders, IStorageProjectionFolder folder)
        {
            var foldersForDelete = from f1 in Folders
                from f2 in folders
                where f1.Account.Login == f2.Account.Login
                select f1;

            foreach (var item in foldersForDelete)
            {
                item.Move(folder);
            }
        }

        public void Copy(IList<IStorageFolder2> folders, IStorageProjectionFolder folder)
        {
            var foldersForDelete = from f1 in Folders
                from f2 in folders
                where f1.Account.Login == f2.Account.Login
                select f1;

            foreach (var item in foldersForDelete)
            {
                item.Copy(folder);
            }
        }

        public void CreatePublicUrl(IList<IStorageFolder2> folders)
        {
            var foldersForDelete = from f1 in Folders
                from f2 in folders
                where f1.Account.Login == f2.Account.Login
                select f1;

            foreach (var item in foldersForDelete)
            {
                item.CreatePublicUrl();
            }
        }

        public void LoadPublicUrl(IList<IStorageFolder2> folders)
        {
            //var foldersForDelete = from f1 in Folders
            //    from f2 in folders
            //    where f1.Account.Login == f2.Account.Login
            //    select f1;

            //foreach (var item in foldersForDelete)
            //{
            //    item.LoadPublicUrl();
            //}
        }

        public void DeletePublicUrl(IList<IStorageFolder2> folders)
        {
            var foldersForDelete = from f1 in Folders
                from f2 in folders
                where f1.Account.Login == f2.Account.Login
                select f1;

            foreach (var item in foldersForDelete)
            {
                item.DeletePublicUrl();
            }
        }
    }
}
