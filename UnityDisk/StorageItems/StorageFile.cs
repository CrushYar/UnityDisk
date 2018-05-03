using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages;

namespace UnityDisk.StorageItems
{
    public class StorageFile:IStorageFile
    {
        public IFileStorageFile DataContext { get; set; }

        public string Name => DataContext.Name;
        public string Path => DataContext.Path;

        public BitmapImage PreviewImage
        {
            get => DataContext.PreviewImage;
            set => DataContext.PreviewImage = value;
        }

        public string PublicUrl => DataContext.PublicUrl;
        public IAccountProjection Account
        {
            get => DataContext.Account;
            set => DataContext.Account = value;
        }
        public DateTime CreateDate => DataContext.CreateDate;
        public long Size => DataContext.Size;
        public string Type => DataContext.Type;
        public StorageItemAttributeEnum Attribute => DataContext.Attribute;
        public StorageItemStateEnum State { get; private set; }
        public IStorageProjectionFolder Parent { get; set; }
        public void Rename(string newName)
        {
            DataContext.Rename(newName);
        }

        public void Delete()
        {
            DataContext.Delete();
        }

        public void Move(IStorageProjectionFolder folder)
        {
           IStorageFolder2 toFolder= folder.Folders.FirstOrDefault(folder2 => folder2.Account.Login == Account.Login);
            if (toFolder == null)
                throw new ArgumentException("Not found Account");
            
            DataContext.Move(toFolder.DataContext);
            
        }

        public void Copy(IStorageProjectionFolder folder)
        {
            IStorageFolder2 toFolder = folder.Folders.FirstOrDefault(folder2 => folder2.Account.Login == Account.Login);
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

        public void Download(Windows.Storage.IStorageFile storageFile)
        {
            DataContext.Download(storageFile);
        }

        public void Parse(string data)
        {
            DataContext.Parse(data);
        }

        public override string ToString()
        {
            return DataContext.ToString();
        }
    }
}
