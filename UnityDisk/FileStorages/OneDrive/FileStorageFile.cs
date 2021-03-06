﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Imaging;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using Unity;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages.OneDrive.Deserialized;
using UnityDisk.StorageItems;
using IStorageFile = Windows.Storage.IStorageFile;

namespace UnityDisk.FileStorages.OneDrive
{
    [Serializable]
    public class FileStorageFile:OneDrive.IFileStorageFile
    {
        public string Id { get; private set; }
        public string PublicUrlId { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        [JsonIgnore]
        public BitmapImage PreviewImage { get; set; }
        public StorageItemTypeEnum Type { get; private set; }
        public string PublicUrl { get; private set; }
        [JsonIgnore]
        public IAccountProjection Account { get; set; }
        [JsonIgnore]
        public DateTime CreateDate { get; private set; }
        public ulong Size { get; private set; }
        public string DownloadUrl { get; private set; }

        public FileStorageFile() { }

        public FileStorageFile(FileBuilder builder)
        {
            Id = builder.Id;
            Name = builder.Name;
            Path = builder.Path;
            PreviewImage = builder.PreviewImage;
            PublicUrl = builder.PublicUrl;
            Account = builder.Account;
            CreateDate = builder.CreateDate;
            Type = builder.Type;
            Size = builder.Size;
            DownloadUrl = builder.DownloadUrl;
        }

        public FileStorageFile(IAccountProjection account)
        {
            Account = account;
        }
        public async Task Delete()
        {
            var httpClient = new System.Net.Http.HttpClient();
            string path = AddBackslash(Path);

            path += Name;
            string url = "https://graph.microsoft.com/v1.0/me" + path;
            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Delete, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);

            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);
            if (response.StatusCode != HttpStatusCode.NoContent) throw new InvalidOperationException("Item did not delete");
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
            string content = "{\r\n  \"parentReference\": {\r\n    \"path\": \"" + Path + "\"\r\n  },\r\n  \"name\": \"" + newName + "\"\r\n}";
            request.Content = new StringContent(content);
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

            string url = "https://graph.microsoft.com/v1.0/me" + fullPathFrom + ":/copy";
            var request = new System.Net.Http.HttpRequestMessage(HttpMethod.Post, url);
            request.Version = Version.Parse("1.0");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);
            string content = "{\r\n  \"parentReference\": {\r\n    \"path\": \"" + pathTo + "\"\r\n  },\r\n  \"name\": \"" + Name + "\"\r\n}";
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
                throw new InvalidOperationException("Item did not copy");

            return null;
        }

        public async Task LoadPreviewImage()
        {
            // Geting url for download
            var httpClient = new System.Net.Http.HttpClient();
            string fullPathFrom = AddBackslash(Path);
            fullPathFrom += Name;

            string url = "https://graph.microsoft.com/v1.0/me" + fullPathFrom + ":/thumbnails/0/medium";
            var request = new System.Net.Http.HttpRequestMessage(HttpMethod.Get, url);
            request.Version = Version.Parse("1.0");
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);
            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);

            string urlImage = null;
            using (System.IO.Stream stream = await response.Content.ReadAsStreamAsync())
            {
                DeserializedImage deserializedImage = new DeserializedImage();

                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedImage.GetType());
                deserializedImage = ser.ReadObject(stream) as DeserializedImage;

                if (deserializedImage == null || String.IsNullOrEmpty(deserializedImage.Url))
                    throw new NullReferenceException("Couldn't deserialized the data");
                urlImage = deserializedImage.Url;
            }

            // Download image
            httpClient = new System.Net.Http.HttpClient();
            request = new System.Net.Http.HttpRequestMessage(HttpMethod.Get, urlImage);
            response = await httpClient.SendAsync(request);

            using (var streamCopy = new MemoryStream())
            {
                using (System.IO.Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    Byte[] bytes = new Byte[1024];
                    int reader = 0;
                    do
                    {
                        reader = await stream.ReadAsync(bytes, 0, bytes.Length);
                        await streamCopy.WriteAsync(bytes, 0, reader);
                    } while (reader > 0);

                    await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Low, async () =>
                    {
                        PreviewImage = new BitmapImage();
                        // ReSharper disable AccessToDisposedClosure
                        streamCopy.Seek(0, SeekOrigin.Begin);
                        await PreviewImage.SetSourceAsync(streamCopy.AsRandomAccessStream());
                        // ReSharper restore AccessToDisposedClosure
                    });
                }
            }
        }

        public async Task LoadPublicUrl()
        {
            if (!String.IsNullOrEmpty(PublicUrl)) return;

            var httpClient = new System.Net.Http.HttpClient();
            string fullPathFrom = AddBackslash(Path);
            fullPathFrom += Name;

            string url = "https://graph.microsoft.com/v1.0/me" + fullPathFrom + ":/permissions";
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

                if (deserializedPublicUrlItem?.Link== null)
                    throw new NullReferenceException("Couldn't deserialized the data");

                PublicUrl = deserializedPublicUrlItem.Link.WebUrl;
                PublicUrlId = deserializedPublicUrlItem.Id;
            }
        }

        public async Task DeletePublicUrl()
        {
            if (String.IsNullOrEmpty(PublicUrl)) return;
            if (String.IsNullOrEmpty(PublicUrlId)) await LoadPublicUrl();
            if (String.IsNullOrEmpty(PublicUrlId)) return;

            var httpClient = new System.Net.Http.HttpClient();
            string fullPathFrom = AddBackslash(Path);
            fullPathFrom += Name;

            string url = "https://graph.microsoft.com/v1.0/me" + fullPathFrom + ":/permissions/" + PublicUrlId;
            var request = new System.Net.Http.HttpRequestMessage(HttpMethod.Delete, url);
            request.Version = Version.Parse("1.0");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Account.Token);
            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException("Item did not create the public url");

            PublicUrlId = PublicUrl = null;
        }

        /// <summary>
        /// Импортирует данные из строки
        /// </summary>
        /// <param name="data">Данные в строковом виде</param>
        /// <returns>Объект полученный после анализа строки</returns>
        public static OneDrive.IFileStorageFile Parse(String data)
        {
            var anonymClass = new { This = "", Login = "" };
            anonymClass = JsonConvert.DeserializeAnonymousType(data, anonymClass);
            FileStorageFile thisClass = JsonConvert.DeserializeObject<FileStorageFile>(anonymClass.This);

            var file= new FileStorageFile(new FileBuilder()
            {
                Name = thisClass.Name,
                DownloadUrl = thisClass.DownloadUrl,
                Id = thisClass.Id,
                Path = thisClass.Path,
                PublicUrl = thisClass.PublicUrl,
                Size = thisClass.Size,
                Type = thisClass.Type
            });
            IUnityContainer container = UnityDisk.ContainerConfiguration.GetContainer().Container;
            var accountRegistry = container.Resolve<UnityDisk.Accounts.Registry.IAccountRegistry>();
            file.Account = accountRegistry.Find(anonymClass.Login);
            return file;
        }

        public override string ToString()
        {
            var result = new {This = JsonConvert.SerializeObject(this), Login=Account.Login};
            return JsonConvert.SerializeObject(result);
        }

        public BackgroundOperation.IDownloader Download(IStorageFile file)
        {
            OneDrive.Downloader downloader=new Downloader(file,this);

            return downloader;
        }

        private string AddBackslash(string path)
        {
            string newPath = path;
            if (newPath[newPath.Length - 1] != '/')
                newPath += "/";

            return newPath;
        }
    }
}
