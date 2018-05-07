using System;
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
    public class FolderBuilder
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public BitmapImage PreviewImage { get; set; }
        public StorageItemAttributeEnum Attribute { get; set; }
        public string PublicUrl { get; set; }
        public IAccountProjection Account { get; set; }
        public DateTime CreateDate { get; set; }
        public string DownloadUrl { get; set; }
        public FolderBuilder() { }

        public FolderBuilder(DeserializedItem item)
        {
            Id = item.id;
            Name = item.name;
            Attribute = StorageItemAttributeEnum.Directory;
            PublicUrl = item.webUrl;
            CreateDate = DateTime.Parse(item.createdDateTime);
            DownloadUrl = item.downloadUrl;
        }
    }
}
