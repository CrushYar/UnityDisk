using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;
using UnityDisk.BackgroundOperation;
using UnityDisk.FileStorages.OneDrive.Deserialized;
using UnityDisk.StorageItems;

namespace UnityDisk.FileStorages.OneDrive
{
    public class Uploader:BackgroundOperation.IUploader
    {

        /// <summary>
        /// Локальный файл
        /// </summary>
        private Windows.Storage.StorageFile _loсalFile;
        private CancellationTokenSource _cancellationToken;
        private DateTime _lastStep;
        private ulong _previewUploadBytes;
        private string _pathToLocalFile;

        public ulong TotalBytesToTransfer { get; private set; }
        public ulong ByteTransferred { get; private set; }
        public ulong Speed { get; private set; }
        public BackgroundOperationActionEnum Action { get; private set; }
        public DateTime DateCompleted { get; private set; }
        public BackgroundOperationStateEnum State { get; private set; }
        public IStorageFile RemoteFile { get; private set; }
        public Task Start()
        {
            throw new NotImplementedException();
        }

        public Uploader(Windows.Storage.StorageFile loсalFile,StorageItems.IStorageFile file)
        {
            RemoteFile = file;
            _loсalFile = loсalFile;
            _pathToLocalFile = _loсalFile.Path;
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }

        public void Step()
        {
            _lastStep = DateTime.Now;
            DateTime currentTime = DateTime.Now;
            var different = currentTime.Subtract(_lastStep);
            ulong downloadBytes = ByteTransferred - _previewUploadBytes;
            _previewUploadBytes = ByteTransferred;
            Speed = (downloadBytes / (ulong)different.Milliseconds)*60;
            _lastStep = DateTime.Now;
        }

        public async void Initialization(IList<UploadOperation> uploaders)
        {
            _loсalFile=await StorageApplicationPermissions.FutureAccessList.GetFileAsync(_loсalFile.Path);
        }

        private async Task Run()
        {
            string fullPath = AddBackslash(RemoteFile.Path);
            fullPath += RemoteFile.Name;

            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post,
                "https://graph.microsoft.com/v1.0/me" + fullPath + ":/createUploadSession");
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", RemoteFile.Account.Token);

            var httpClient = new System.Net.Http.HttpClient();

            var response = await httpClient.SendAsync(request);

            DeserializedUploadSession deserializedUploadSession = new DeserializedUploadSession();
            using (System.IO.Stream stream = await response.Content.ReadAsStreamAsync())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUploadSession.GetType());
                deserializedUploadSession = ser.ReadObject(stream) as DeserializedUploadSession;

                if (deserializedUploadSession?.UploadUrl == null)
                    throw new NullReferenceException("Couldn't deserialized the data");
            }

            var fileStream = await _loсalFile.OpenReadAsync();

            uint bytesCount = 10 * 320 * 1024;
            Byte[] bytes = new byte[bytesCount];
            Regex regex = new Regex("[0-9]*");
            var result = regex.Match(deserializedUploadSession.NextExpectedRanges[0]);
            _previewUploadBytes = UInt32.Parse(result.Value);
            fileStream.Seek(_previewUploadBytes);
            ulong offset = _previewUploadBytes;
            string token = RemoteFile.Account.Token;
            do
            {
                request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Put,
                    "https://api.onedrive.com/rup/ba580bfa74ca04b7/eyJSZXNvdXJjZUlEIjoiQkE1ODBCRkE3NENBMDRCNyE0NjAiLCJSZWxhdGlvbnNoaXBOYW1lIjoiTHVpcyBGb25zaSAtIERlc3BhY2l0byBmdC4gRGFkZHkgWWFua2VlLm1wNCJ9/4mljKc7vvf3kk1vsmDZEJjbHFb9FNrrJA3_JjfI3yvSYfVaqeS8JTrJJ2QocG2o0qCYlpNdulraqLrKpkz0MvzjVr826tgHza_Y9v9YOWebeo/eyJOYW1lIjoiTHVpcyBGb25zaSAtIERlc3BhY2l0byBmdC4gRGFkZHkgWWFua2VlLm1wNCJ9/4wli1SXBXp-I4-vuiB4bI1R25OWHjC2hIVvkqkv7YcT-jeNSrHcg2HK7zRRmjp0z9PUJrz0rscSIt8p2SBcVxsfYDa9yZrAblEj5CIFdWNfCkjfbIoYNJbRcou7QmVj4P-hJ7J_ewbQSw1Osrbqiz_tdrHGOZ00r4UtAvDD-0Ruh32xC4wI74Ou0gKeh8ZHfeyntrvavayiMynes3SyRCyJUY7tXhO0VhyYBT0fhSAomDhKE_5hanj81DRr_wUsB3d33VoWDXGf7zUUKSEqlEhKipJtGlKsW7kadtnN4J7UYSi4JleQzIfCQO9k4aHeuHBg090u6lhRE7VOzCP9mf1UZ5YVUZ_UTYLNiuY9SSCAjXA6DkclJqvtRV11aw6xiPcxzAS8tPcXrn-GyoUT5SWX9aRli3HncLRNKZH4_n7Qls7IQHf_Gc1ccI4pHSeGJJk6ZVmwZJ5xB8YX0BQV8ZGue-byClNjh0FgL3Ag2VfUqf5Y5pglEO7kidhfCW4IvUrpK8-cG6KZD-vNj2GWhv0Zg");
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var reader = await fileStream.ReadAsync(bytes.AsBuffer(), bytesCount, InputStreamOptions.None);
                request.Content = new ByteArrayContent(reader.ToArray());
                request.Content.Headers.ContentRange = new ContentRangeHeaderValue((long)offset, (long)offset + reader.Length - 1,
                    (long) TotalBytesToTransfer);
                response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                offset += reader.Length;
            } while (offset != TotalBytesToTransfer);

            StorageApplicationPermissions.FutureAccessList.Remove(_loсalFile.Path);
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
