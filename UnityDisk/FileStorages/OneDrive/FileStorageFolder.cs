using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public class FileStorageFolder: OneDrive.IFileStorageFolder
    {
        public string Id { get; private set; }
        public string PublicUrlId { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public BitmapImage PreviewImage { get; set; }
        public StorageItemTypeEnum Type => StorageItemTypeEnum.Directory;
        public string PublicUrl { get; private set; }
        public IAccountProjection Account { get; set; }
        public DateTime CreateDate { get; private set; }
        public string DownloadUrl { get; private set; }

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
            string path = AddBackslash(Path);

            path += Name;
            string url = "https://graph.microsoft.com/v1.0/me"+ path;
           var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Delete, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);

            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);
            if(response.StatusCode!= HttpStatusCode.NoContent) throw new InvalidOperationException("Item did not delete");
        }

        public async Task Rename(string newName)
        {
            var httpClient = new System.Net.Http.HttpClient();
            string fullPathFrom = AddBackslash(Path);

            fullPathFrom += Name;
            string url = "https://graph.microsoft.com/v1.0/me" + fullPathFrom;
            var request = new System.Net.Http.HttpRequestMessage(new HttpMethod("PATCH"), url);
            request.Version = Version.Parse("1.0");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);
            string content= "{\r\n  \"parentReference\": {\r\n    \"path\": \""+ Path + "\"\r\n  },\r\n  \"name\": \""+ newName+"\"\r\n}";
             request.Content=new StringContent(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);

            string test = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK) throw new InvalidOperationException("Item did not rename");
            Name = newName;
        }

        public async Task Move(FileStorages.IFileStorageFolder folder)
        {
            var httpClient = new System.Net.Http.HttpClient();
            string fullPathFrom = AddBackslash(Path);
            fullPathFrom += Name;

            string pathTo = AddBackslash(folder.Path);
            pathTo += folder.Name;

            string url = "https://graph.microsoft.com/v1.0/me" + fullPathFrom;
            var request = new System.Net.Http.HttpRequestMessage(new HttpMethod("PATCH"), url);
            request.Version = Version.Parse("1.0");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);
            string content = "{\r\n  \"parentReference\": {\r\n    \"path\": \"" + pathTo + "\"\r\n  },\r\n  \"name\": \"" + Name + "\"\r\n}";
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);

            string test = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK) throw new InvalidOperationException("Item did not move");

            Path = pathTo;
        }

        public async Task<FileStorages.IFileStorageItem> Copy(FileStorages.IFileStorageFolder othePath)
        {
            var httpClient = new System.Net.Http.HttpClient();
            string fullPathFrom = AddBackslash(Path);
            fullPathFrom += Name;

            string pathTo = AddBackslash(othePath.Path);
            pathTo += othePath.Name;

            string url = "https://graph.microsoft.com/v1.0/me" + fullPathFrom+ ":/copy";
            var request = new System.Net.Http.HttpRequestMessage(HttpMethod.Post, url);
            request.Version = Version.Parse("1.0");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);
            string content = "{\r\n  \"parentReference\": {\r\n    \"path\": \"" + pathTo + "\"\r\n  },\r\n  \"name\": \"" + Name + "\"\r\n}";
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK&& response.StatusCode != HttpStatusCode.Accepted)
                throw new InvalidOperationException("Item did not copy");

            return await othePath.CreateFolder(Name);

        }

        public Task LoadPreviewImage()
        {
            throw new NotImplementedException();
        }

        public async Task LoadPublicUrl()
        {
            if (!String.IsNullOrEmpty(PublicUrl)) return;

            var httpClient = new System.Net.Http.HttpClient();
            string fullPathFrom = AddBackslash(Path);
            fullPathFrom += Name;

            string url = "https://graph.microsoft.com/v1.0/me" + fullPathFrom+ ":/permissions";
            var request = new System.Net.Http.HttpRequestMessage(HttpMethod.Get, url);
            request.Version = Version.Parse("1.0");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);
            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException("Item did not load the public url");

            using (System.IO.Stream stream = await response.Content.ReadAsStreamAsync())
            {
                DeserializedPublicUrl deserializedPublicUrl = new DeserializedPublicUrl();

                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedPublicUrl.GetType());
                deserializedPublicUrl = ser.ReadObject(stream) as DeserializedPublicUrl;

                if (deserializedPublicUrl?.value == null)
                    throw new NullReferenceException("Couldn't deserialized the data");

                var link = deserializedPublicUrl.value.FirstOrDefault(item => item.Link.Application?.DisplayName == "UnityDisk");

                PublicUrl = link?.Link.WebUrl;
                PublicUrlId = link?.Id;
            }
        }

        public async Task CreatePublicUrl()
        {
            if (!String.IsNullOrEmpty(PublicUrl)) return;

            var httpClient = new System.Net.Http.HttpClient();
            string fullPathFrom = AddBackslash(Path);
            fullPathFrom += Name;

            string url = "https://graph.microsoft.com/v1.0/me" + fullPathFrom + ":/createLink";
            var request = new System.Net.Http.HttpRequestMessage(HttpMethod.Post, url);
            request.Version = Version.Parse("1.0");
            string content = "{\r\n\t\"type\": \"view\"\r\n}";
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);
            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException("Item did not create the public url");

            using (System.IO.Stream stream = await response.Content.ReadAsStreamAsync())
            {
                DeserializedPublicUrlItem deserializedPublicUrlItem = new DeserializedPublicUrlItem();

                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedPublicUrlItem.GetType());
                deserializedPublicUrlItem = ser.ReadObject(stream) as DeserializedPublicUrlItem;

                if (deserializedPublicUrlItem?.Link == null)
                    throw new NullReferenceException("Couldn't deserialized the data");

                PublicUrl = deserializedPublicUrlItem.Link.WebUrl;
                PublicUrlId = deserializedPublicUrlItem.Id;
            }
        }

        public async Task DeletePublicUrl()
        {
            if (String.IsNullOrEmpty(PublicUrl)) return;
            if (String.IsNullOrEmpty(PublicUrlId)) await LoadPublicUrl();
            if (String.IsNullOrEmpty(PublicUrlId))return;

            var httpClient = new System.Net.Http.HttpClient();
            string fullPathFrom = AddBackslash(Path);
            fullPathFrom += Name;

            string url = "https://graph.microsoft.com/v1.0/me" + fullPathFrom+ ":/permissions/" + PublicUrlId;
            var request = new System.Net.Http.HttpRequestMessage(HttpMethod.Delete, url);
            request.Version = Version.Parse("1.0");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);
            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException("Item did not create the public url");

            PublicUrlId = PublicUrl = null;
        }

        public void Parse(string data)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<FileStorages.IFileStorageItem>> LoadDirectory()
        {
            List<FileStorages.IFileStorageItem> result=new List<FileStorages.IFileStorageItem>();
            DeserializedItemList deserializedItemList = new DeserializedItemList();
            string path = string.IsNullOrEmpty(Path) ? "/drive/root" : String.Concat("{0}{1}:",Path,Name);
            using (var stream = await GetDataStream("https://graph.microsoft.com/v1.0/me"+path+"/children"))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedItemList.GetType());
                deserializedItemList = ser.ReadObject(stream) as DeserializedItemList;
            }
            if (deserializedItemList == null) throw new NullReferenceException("Couldn't deserialized the data");

            foreach (var item in deserializedItemList.Value)
            {
                FileStorages.IFileStorageItem storageItem;
                if (item.Folder != null)
                    storageItem =new FileStorageFolder(new FolderBuilder(item));
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

        public async Task<FileStorages.IFileStorageFolder> CreateFolder(string name)
        {
            var httpClient = new System.Net.Http.HttpClient();

            string fullPath;
            if (String.IsNullOrEmpty(Name))
                fullPath = "/drive/root";
            else
            {
                fullPath = AddBackslash(Path);
                fullPath += Name+":";
            }

            string url = "https://graph.microsoft.com/v1.0/me"+ fullPath+"/children";
            var request = new System.Net.Http.HttpRequestMessage(HttpMethod.Post, url);
            request.Version = Version.Parse("1.0");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);
            string content = "{\r\n  \"name\": \""+name+"\",\r\n  \"folder\": { }\r\n}";
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                throw new InvalidOperationException("Item did not create");
            DeserializedItem deserializedItem = new DeserializedItem();

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedItem.GetType());
                deserializedItem = ser.ReadObject(stream) as DeserializedItem;
            }
            if (deserializedItem == null) throw new NullReferenceException("Couldn't deserialized the data");

            return new FileStorageFolder(new FolderBuilder(deserializedItem){PreviewImage = PreviewImage,Account = Account});
        }

        private async Task<System.IO.Stream> GetDataStream(string url)
        {
            var httpClient = new System.Net.Http.HttpClient();

            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);

            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException("Item did not copy");

            return await response.Content.ReadAsStreamAsync();
        }

        private string AddBackslash(string path)
        {
            string newPath = Path;
            if (newPath[newPath.Length - 1] != '/')
                newPath += "/";

            return newPath;
        }
    }
}
