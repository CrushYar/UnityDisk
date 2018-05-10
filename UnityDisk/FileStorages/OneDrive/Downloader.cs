using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;
using UnityDisk.BackgroundOperation;
using UnityDisk.StorageItems;

namespace UnityDisk.FileStorages.OneDrive
{
    public class Downloader:BackgroundOperation.IDownloader
    {
        /// <summary>
        /// Локальный файл
        /// </summary>
        private Windows.Storage.StorageFile _loсalFile;
        private DownloadOperation _downloadOperation;
        private CancellationTokenSource _cancellationToken;
        private DateTime _lastStep;
        private ulong _previewDownloadBytes;
        private int _proccessId;

        public ulong TotalBytesToTransfer { get; private set; }
        public ulong ByteTransferred { get; private set; }
        public ulong Speed { get; private set; }
        public BackgroundOperationActionEnum Action { get; private set; }
        public DateTime DateCompleted { get; private set; }
        public BackgroundOperationStateEnum State { get; private set; }
        public IStorageFile RemoteFile { get; private set; }
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
            DateTime currentTime= DateTime.Now;
            var different= currentTime.Subtract(_lastStep);
            ulong currentDownloaded = _previewDownloadBytes + _downloadOperation.Progress.BytesReceived;
            ulong downloadBytes =currentDownloaded - ByteTransferred;
            ByteTransferred = currentDownloaded;
            Speed = (downloadBytes / (ulong)different.Milliseconds)*60;
            _lastStep=DateTime.Now;
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
            _loсalFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(_loсalFile.Path);
            var basicProperties= await _loсalFile.GetBasicPropertiesAsync();
            _previewDownloadBytes = (uint)basicProperties.Size;
        }

        private async Task Run()
        {
            var httpClient = new System.Net.Http.HttpClient();
            string fullPath = AddBackslash(RemoteFile.Path);
            fullPath += RemoteFile.Name;

            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, "https://graph.microsoft.com/v1.0/me"+ fullPath+ ":/content");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", RemoteFile.Account.Token);
            BackgroundDownloader downloader= new BackgroundDownloader();
            _downloadOperation = downloader.CreateDownload(new Uri("https://graph.microsoft.com/v1.0/me" + fullPath + ":/content"), _loсalFile);
            _cancellationToken=new CancellationTokenSource();
            _proccessId = _downloadOperation.Guid.GetHashCode();
            await _downloadOperation.StartAsync().AsTask(_cancellationToken.Token);
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
