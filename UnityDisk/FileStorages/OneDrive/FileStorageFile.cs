using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;
using UnityDisk.StorageItems;
using IStorageFile = Windows.Storage.IStorageFile;

namespace UnityDisk.FileStorages.OneDrive
{
    public class FileStorageFile:OneDrive.IFileStorageFile
    {
        public string Id { get; set; }
        public string Name { get; }
        public string Path { get; }
        public BitmapImage PreviewImage { get; set; }
        public StorageItemTypeEnum Type { get; }
        public string PublicUrl { get; }
        public IAccountProjection Account { get; set; }
        public DateTime CreateDate { get; }
        public ulong Size { get; }

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public FileStorageFile() { }

        public FileStorageFile(FileBuilder builder)
        {
            Name = builder.Name;
            Path = builder.Path;
            PreviewImage = builder.PreviewImage;
            PublicUrl = builder.PublicUrl;
            Account = builder.Account;
            CreateDate = builder.CreateDate;
            Type = builder.Type;
            Size = builder.Size;
        }

        public FileStorageFile(IAccountProjection account)
        {
            Account = account;
        }

        public Task Rename(string newName)
        {
            throw new NotImplementedException();
        }

        public Task Move(FileStorages.IFileStorageFolder folder)
        {
            throw new NotImplementedException();
        }

        public Task<FileStorages.IFileStorageItem> Copy(FileStorages.IFileStorageFolder othePath)
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

        public Task Download(IStorageFile file)
        {
            throw new NotImplementedException();
        }
    }
}
