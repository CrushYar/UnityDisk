using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media.Imaging;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages.OneDrive.Deserialized;
using UnityDisk.StorageItems;
using IStorageFile = Windows.Storage.IStorageFile;

namespace UnityDisk.FileStorages.OneDrive
{
    public class FileStorageFolder:IFileStorageFolder
    {
        public string Id { get; set; }
        public string Name { get; }
        public string Path { get; }
        public BitmapImage PreviewImage { get; set; }
        public StorageItemAttributeEnum Attribute => StorageItemAttributeEnum.Directory;
        public string PublicUrl { get; }
        public IAccountProjection Account { get; set; }
        public DateTime CreateDate { get; }
        public string DownloadUrl { get; set; }

        public FileStorageFolder() { }

        public FileStorageFolder(FolderBuilder folderBuilder)
        {
            Name = folderBuilder.Name;
            Path = folderBuilder.Path;
            PreviewImage = folderBuilder.PreviewImage;
            PublicUrl = folderBuilder.PublicUrl;
            Account = folderBuilder.Account;
            CreateDate = folderBuilder.CreateDate;
            DownloadUrl = folderBuilder.DownloadUrl;
            Id = folderBuilder.Id;
        }

        public FileStorageFolder(IAccountProjection account)
        {
            Account = account;
        }
        public async Task Delete()
        {
            var httpClient = new System.Net.Http.HttpClient();
            string path = Path;
            if (path[path.Length - 1] != '/')
                path += "/";

            path += Name;
            string url = "https://graph.microsoft.com/v1.0/me"+ path;
           var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Delete, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);

            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);
            if(response.StatusCode!= HttpStatusCode.NoContent) throw new InvalidOperationException("Item did not delete");
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

        public async Task<IList<IFileStorageItem>> LoadDirectory()
        {
            List<IFileStorageItem> result=new List<IFileStorageItem>();
            DeserializedItemList deserializedItemList = new DeserializedItemList();
            string path = string.IsNullOrEmpty(Path) ? "/drive/root" : String.Concat("{0}{1}:",Path,Name);
            using (var stream = await GetDataStream("https://graph.microsoft.com/v1.0/me"+path+"/children"))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedItemList.GetType());
                deserializedItemList = ser.ReadObject(stream) as DeserializedItemList;
            }
            if (deserializedItemList == null) throw new NullReferenceException("Couldn't deserialized the data");

            foreach (var item in deserializedItemList.value)
            {
                IFileStorageItem storageItem;
                if (item.folder != null)
                    storageItem =new FileStorageFolder(new FolderBuilder(item){PreviewImage = PreviewImage});
                else
                    storageItem = new FileStorageFile(new FileBuilder(item));
                result.Add(storageItem);
            }

            return result;
        }

        public Task Upload(IStorageFile loacalFile)
        {
            throw new NotImplementedException();
        }

        public async Task<IFileStorageFolder> CreateFolder(string name)
        {
            throw new NotImplementedException();
        }

        private async Task<System.IO.Stream> GetDataStream(string url)
        {
            var httpClient = new System.Net.Http.HttpClient();

            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);

            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);
            string test = await response.Content.ReadAsStringAsync();
            return await response.Content.ReadAsStreamAsync();
        }
    }
}
