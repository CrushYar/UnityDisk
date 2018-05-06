using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using UnityDisk.Accounts.Registry;
using UnityDisk.StorageItems;
using IStorageFile = Windows.Storage.IStorageFile;

namespace UnityDisk.FileStorages.OneDrive
{
    public class FileStorageFolder:IFileStorageFolder
    {
        public string Name { get; }
        public string Path { get; }
        public BitmapImage PreviewImage { get; set; }
        public StorageItemAttributeEnum Attribute { get; }
        public string PublicUrl { get; }
        public IAccountProjection Account { get; set; }
        public DateTime CreateDate { get; }
        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public Task Rename(string newName)
        {
            throw new NotImplementedException();
        }

        public Task Move(IFileStorageFolder folder)
        {
            throw new NotImplementedException();
        }

        public Task Copy(IFileStorageFolder othePath)
        {
            throw new NotImplementedException();
        }

        public Task LoadPreviewImage()
        {
            throw new NotImplementedException();
        }

        public Task LoadPublicUrl()
        {
            throw new NotImplementedException();
        }

        public Task CreatePublicUrl()
        {
            throw new NotImplementedException();
        }

        public Task DeletePublicUrl()
        {
            throw new NotImplementedException();
        }

        public void Parse(string data)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IFileStorageItem>> LoadDirectory()
        {
            throw new NotImplementedException();
        }

        public Task Upload(IStorageFile loacalFile)
        {
            throw new NotImplementedException();
        }

        public Task<IFileStorageFolder> CreateFolder(string name)
        {
            throw new NotImplementedException();
        }
    }
}
