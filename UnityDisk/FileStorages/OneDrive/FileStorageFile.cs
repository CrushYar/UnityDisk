using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Imaging;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages.OneDrive.Deserialized;
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

                if (deserializedImage == null || String.IsNullOrEmpty(deserializedImage.url))
                    throw new NullReferenceException("Couldn't deserialized the data");
                urlImage = deserializedImage.url;
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

        private string AddBackslash(string path)
        {
            string newPath = Path;
            if (newPath[newPath.Length - 1] != '/')
                newPath += "/";

            return newPath;
        }
    }
}
