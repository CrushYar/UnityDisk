﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages.OneDrive.Deserialized;
using UnityDisk.StorageItems;

namespace UnityDisk.FileStorages.OneDrive
{
    public class FileBuilder
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public BitmapImage PreviewImage { get; set; }
        public StorageItemTypeEnum Type { get; set; }
        public string PublicUrl { get; set; }
        public IAccountProjection Account { get; set; }
        public DateTime CreateDate { get; set; }
        public string DownloadUrl { get; set; }
        public ulong Size { get; set; }

        public FileBuilder() { }

        public FileBuilder(DeserializedItem item)
        {
            Id = item.Id;
            Name = item.Name;
            if (item.File != null)
                Type = FileStorages.Convertor.ToStorageItemType(item.File.MimeType);
            else
                Type = FileStorages.Convertor.ToStorageItemType(item.Package.Type);
            
            Size = item.Size;
            PublicUrl = item.WebUrl;
            CreateDate = DateTime.Parse(item.CreatedDateTime);
            DownloadUrl = item.DownloadUrl;
            Path = item.ParentReference.Path;
        }
    }
}
