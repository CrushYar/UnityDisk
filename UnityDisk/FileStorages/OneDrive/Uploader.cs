﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json;
using UnityDisk.BackgroundOperation;
using UnityDisk.FileStorages.OneDrive.Deserialized;
using UnityDisk.StorageItems;

namespace UnityDisk.FileStorages.OneDrive
{
    [Serializable]
    public class Uploader:BackgroundOperation.IUploader
    {

        /// <summary>
        /// Локальный файл
        /// </summary>
        [NonSerialized]
        private Windows.Storage.IStorageFile _loсalFile;
        [NonSerialized]
        private CancellationTokenSource _cancellationToken;
        [NonSerialized]
        private DateTime _lastStep;
        private ulong _previewUploadBytes;
        private string _pathToLocalFile;
        [NonSerialized]
        private readonly OneDrive.IFileStorageFolder _storageFolder;
        private string _uploadUrl;
        private string _localFileToken;
         
        public ulong TotalBytesToTransfer { get; private set; }
        public ulong ByteTransferred { get; private set; }
        public ulong Speed { get; private set; }
        [JsonIgnore]
        public BackgroundOperationActionEnum Action => BackgroundOperationActionEnum.Upload;
        [JsonIgnore]
        public DateTime DateCompleted { get; private set; }
        public BackgroundOperationStateEnum State { get; private set; }
        [JsonIgnore]
        public UnityDisk.FileStorages.IFileStorageFile RemoteFile { get; private set; }
        public async Task Start()
        {
            string fullPath = AddBackslash(_storageFolder.Path);
            fullPath += _loсalFile.Name;

            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post,
                "https://graph.microsoft.com/v1.0/me" + fullPath + ":/createUploadSession");
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _storageFolder.Account.Token);

            var httpClient = new System.Net.Http.HttpClient();

            var response = await httpClient.SendAsync(request);

            if(response.StatusCode!=HttpStatusCode.Created && response.StatusCode!=HttpStatusCode.OK)
                throw new InvalidOperationException("Session of upload did not create");

            DeserializedUploadSession deserializedUploadSession = new DeserializedUploadSession();
            using (System.IO.Stream stream = await response.Content.ReadAsStreamAsync())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUploadSession.GetType());
                deserializedUploadSession = ser.ReadObject(stream) as DeserializedUploadSession;

                if (deserializedUploadSession?.UploadUrl == null)
                    throw new NullReferenceException("Couldn't deserialized the data");
            }

            _uploadUrl = deserializedUploadSession.UploadUrl;
            await Run();
        }

        public Uploader(Windows.Storage.IStorageFile loсalFile,ulong fileSize, OneDrive.IFileStorageFolder folder)
        {
            _storageFolder = folder;
            _loсalFile = loсalFile;
            _pathToLocalFile = _loсalFile.Path;
            TotalBytesToTransfer = fileSize;
            _localFileToken= StorageApplicationPermissions.FutureAccessList.Add(loсalFile);
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
            ulong uploadedBytes = ByteTransferred - _previewUploadBytes;
            _previewUploadBytes = ByteTransferred;
            if (uploadedBytes > 0)
                Speed = (uploadedBytes / (ulong) different.Milliseconds) * 60;
            else
                Speed = 0;

            _lastStep = DateTime.Now;
        }

        public async void Initialization(IList<UploadOperation> uploaders)
        {
            _loсalFile =await StorageApplicationPermissions.FutureAccessList.GetFileAsync(_localFileToken);
        }

        private async Task Run()
        {
            _cancellationToken=new CancellationTokenSource();
            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get,_uploadUrl);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _storageFolder.Account.Token);

            var httpClient = new System.Net.Http.HttpClient();

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException("Session of upload did not create");
            DeserializedUploadSession deserializedUploadSession = new DeserializedUploadSession();

            using (System.IO.Stream stream = await response.Content.ReadAsStreamAsync())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUploadSession.GetType());
                deserializedUploadSession = ser.ReadObject(stream) as DeserializedUploadSession;

                if (deserializedUploadSession?.NextExpectedRanges == null)
                    throw new NullReferenceException("Couldn't deserialized the data");
            }

            var fileStream = await _loсalFile.OpenReadAsync();

            uint bytesCount = 13*320*1024;
            Byte[] bytes = new byte[bytesCount];
            Regex regex = new Regex("[0-9]*");
            var result = regex.Match(deserializedUploadSession.NextExpectedRanges[0]);
            _previewUploadBytes = UInt32.Parse(result.Value);
            fileStream.Seek(_previewUploadBytes);
            ulong offset = _previewUploadBytes;
            string token = _storageFolder.Account.Token;
            do
            {
                request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Put, _uploadUrl);
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var reader = await fileStream.ReadAsync(bytes.AsBuffer(), bytesCount, InputStreamOptions.None);
                request.Content = new ByteArrayContent(reader.ToArray());
                request.Content.Headers.ContentRange = new ContentRangeHeaderValue((long)offset, (long)offset + reader.Length - 1,
                    (long) TotalBytesToTransfer);
                response = await httpClient.SendAsync(request);
                offset += reader.Length;
            } while (offset != TotalBytesToTransfer&& !_cancellationToken.IsCancellationRequested);

            if (_cancellationToken.IsCancellationRequested)
            {
                StorageApplicationPermissions.FutureAccessList.Remove(_localFileToken);
                return;
            }
            DeserializedItem deserializedItem = new DeserializedItem();

            using (System.IO.Stream stream = await response.Content.ReadAsStreamAsync())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedItem.GetType());
                deserializedItem = ser.ReadObject(stream) as DeserializedItem;

                if (deserializedItem?.File == null)
                    throw new NullReferenceException("Couldn't deserialized the data");
            }
            RemoteFile=new FileStorageFile(new FileBuilder(deserializedItem)){Account = _storageFolder.Account};

            StorageApplicationPermissions.FutureAccessList.Remove(_localFileToken);
        }

        private string AddBackslash(string path)
        {
            string newPath = path;
            if (newPath[newPath.Length - 1] != '/')
                newPath += "/";

            return newPath;
        }
        /// <summary>
        /// Импортирует данные из строки
        /// </summary>
        /// <param name="data">Данные в строковом виде</param>
        /// <returns>Объект полученный после анализа строки</returns>
        public static OneDrive.Uploader Parse(String data)
        {
            var anonymClass = new
            {
                This = "",
                LocalFileToken = "",
                ProccessId = 0,
                PreviewDownloadBytes = (ulong)0,
                RemoteFile = "",
                UploadUrl=""
            };
            anonymClass = JsonConvert.DeserializeAnonymousType(data, anonymClass);
            Uploader uploader = JsonConvert.DeserializeObject<Uploader>(anonymClass.This);
            uploader._localFileToken = anonymClass.LocalFileToken;
            uploader.RemoteFile = OneDrive.FileStorageFile.Parse(anonymClass.RemoteFile);
            uploader._uploadUrl = anonymClass.UploadUrl;
            return uploader;
        }

        public override string ToString()
        {
            var result = new
            {
                This = JsonConvert.SerializeObject(this),
                LocalFileToken = _localFileToken,
                RemoteFile = JsonConvert.SerializeObject(RemoteFile),
                UploadUrl= _uploadUrl
            };
            return JsonConvert.SerializeObject(result);
        }
    }
}
