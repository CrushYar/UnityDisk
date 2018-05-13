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
        public ulong Size => DataContext.Size;
        public StorageItemTypeEnum Type => DataContext.Type;
        public StorageItemStateEnum State { get; private set; }
        public IStorageProjectionFolder Parent { get; set; }
        public async Task Rename(string newName)
        {
            await DataContext.Rename(newName);
        }

        public async Task Delete()
        {
            await DataContext.Delete();
        }

        public async Task Move(IStorageProjectionFolder folder)
        {
           IStorageFolder2 toFolder= folder.Folders.FirstOrDefault(folder2 => folder2.Account.Login == Account.Login);
            if (toFolder == null)
                throw new ArgumentException("Not found Account");

            await DataContext.Move(toFolder.DataContext);
            
        }

        public async Task<IStorageItem> Copy(IStorageProjectionFolder folder)
        {
            IStorageFolder2 toFolder = folder.Folders.FirstOrDefault(folder2 => folder2.Account.Login == Account.Login);
            if (toFolder == null)
                throw new ArgumentException("Not found Account");
            IFileStorageFile storageFile = await DataContext.Copy(toFolder.DataContext) as IFileStorageFile;
            if (storageFile == null)
                throw new NullReferenceException("Did not get file after copy");

            return  new StorageFile()
            {
                DataContext = storageFile,
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

        public BackgroundOperation.IDownloader Download(Windows.Storage.IStorageFile storageFile)
        {
            return DataContext.Download(storageFile);
        }

        //public static StorageFile Parse(string data)
        //{

        //    DataContext.Parse(data);
        //}

        public override string ToString()
        {
            return DataContext.ToString();
        }
    }
}
