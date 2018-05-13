using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using UnityDisk.BackgroundOperation;
using UnityDisk.StorageItems;

namespace UnityDisk.FileStorages.OneDrive
{
    [Serializable]
    public class Downloader:BackgroundOperation.IDownloader
    {
        /// <summary>
        /// Локальный файл
        /// </summary>
        [NonSerialized]
        private Windows.Storage.IStorageFile _loсalFile;
        [NonSerialized]
        private DownloadOperation _downloadOperation;
        [NonSerialized]
        private CancellationTokenSource _cancellationToken;
        [NonSerialized]
        private DateTime _lastStep;
        private ulong _previewDownloadBytes;
        private int _proccessId;
        private string _localFileToken;


        public ulong TotalBytesToTransfer { get; private set; }
        public ulong ByteTransferred { get; private set; }
        public ulong Speed { get; private set; }
        [JsonIgnore]
        public BackgroundOperationActionEnum Action => BackgroundOperationActionEnum.Download;
        [JsonIgnore]
        public DateTime DateCompleted { get; private set; }
        public BackgroundOperationStateEnum State { get; private set; }
        [JsonIgnore]
        public UnityDisk.FileStorages.IFileStorageFile RemoteFile { get; private set; }
        public Downloader(Windows.Storage.IStorageFile loсalFile, OneDrive.IFileStorageFile file)
        {
            RemoteFile = file;
            _loсalFile = loсalFile;
            TotalBytesToTransfer = RemoteFile.Size;
            _localFileToken = StorageApplicationPermissions.FutureAccessList.Add(loсalFile);
        }
        public async Task Start()
        {
            await Run();
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
            ulong currentDownloaded = _previewDownloadBytes + _downloadOperation.Progress.BytesReceived;
            ulong downloadBytes = currentDownloaded - ByteTransferred;
            ByteTransferred = currentDownloaded;
            if (downloadBytes > 0)
                Speed = (downloadBytes / (ulong) different.Milliseconds) * 60;
            else
                Speed = 0;

            _lastStep = DateTime.Now;
            if (ByteTransferred == TotalBytesToTransfer)
            {
                try
                {
                    StorageApplicationPermissions.FutureAccessList.Remove(_localFileToken);
                }
                catch (Exception e)
                {
                }
            }
        }

        public async void Initialization(IList<DownloadOperation> downloaders)
        {
            foreach (var downloader in downloaders)
            {
                if (downloader.Guid.GetHashCode() == _proccessId)
                {
                    _downloadOperation = downloader;
                    break;
                }
            }
            _loсalFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(_localFileToken);
            var basicProperties = await _loсalFile.GetBasicPropertiesAsync();
            _previewDownloadBytes = (uint)basicProperties.Size;
        }

        private async Task Run()
        {
            if (_downloadOperation != null)
            {
                _cancellationToken=new CancellationTokenSource();
                await _downloadOperation.AttachAsync().AsTask(_cancellationToken.Token);
            }
            else
            {
                string fullPath = AddBackslash(RemoteFile.Path);
                fullPath += RemoteFile.Name;

                BackgroundDownloader downloader = new BackgroundDownloader();
                downloader.SetRequestHeader("Authorization", "Bearer " + RemoteFile.Account.Token);
                _downloadOperation =
                    downloader.CreateDownload(new Uri("https://graph.microsoft.com/v1.0/me" + fullPath + ":/content"),
                        _loсalFile);
                _cancellationToken = new CancellationTokenSource();
                _proccessId = _downloadOperation.Guid.GetHashCode();
                await _downloadOperation.StartAsync().AsTask(_cancellationToken.Token);
            }

            try
            {
                StorageApplicationPermissions.FutureAccessList.Remove(_localFileToken);
            }
            catch (Exception e)
            {
            }
        }
        private string AddBackslash(string path)
        {
            string newPath = path;
            if (newPath[newPath.Length - 1] != '/')
                newPath += "/";

            return newPath;
        }
        private Downloader() { }
        /// <summary>
        /// Импортирует данные из строки
        /// </summary>
        /// <param name="data">Данные в строковом виде</param>
        /// <returns>Объект полученный после анализа строки</returns>
        public static OneDrive.Downloader Parse(String data)
        {
            var anonymClass = new
            {
                This = "",
                LocalFileToken = "",
                ProccessId = 0,
                PreviewDownloadBytes = (ulong)0,
                RemoteFile = ""
            };
            anonymClass = JsonConvert.DeserializeAnonymousType(data, anonymClass);
            Downloader downloader = JsonConvert.DeserializeObject<Downloader>(anonymClass.This);
            downloader._proccessId = anonymClass.ProccessId;
            downloader._previewDownloadBytes = anonymClass.PreviewDownloadBytes;
            downloader._localFileToken = anonymClass.LocalFileToken;
            downloader.RemoteFile = OneDrive.FileStorageFile.Parse(anonymClass.RemoteFile);
            return downloader;
        }

        public override string ToString()
        {
            var result = new
            {
                This = JsonConvert.SerializeObject(this),
                LocalFileToken = _localFileToken,
                ProccessId = _proccessId,
                PreviewDownloadBytes = _previewDownloadBytes,
                RemoteFile=JsonConvert.SerializeObject(RemoteFile)
            };
            return JsonConvert.SerializeObject(result);
        }
    }
}
