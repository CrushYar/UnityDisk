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
using UnityDisk.BackgroundOperation;
using UnityDisk.StorageItems;

namespace UnityDisk.FileStorages.OneDrive
{
    public class Downloader:BackgroundOperation.IDownloader
    {
        /// <summary>
        /// Локальный файл
        /// </summary>
        private Windows.Storage.IStorageFile _loсalFile;
        private DownloadOperation _downloadOperation;
        private CancellationTokenSource _cancellationToken;
        private DateTime _lastStep;
        private ulong _previewDownloadBytes;
        private int _proccessId;
        private readonly string _pathToLocalFile;


        public ulong TotalBytesToTransfer { get; private set; }
        public ulong ByteTransferred { get; private set; }
        public ulong Speed { get; private set; }
        public BackgroundOperationActionEnum Action { get; private set; }
        public DateTime DateCompleted { get; private set; }
        public BackgroundOperationStateEnum State { get; private set; }
        public UnityDisk.FileStorages.IFileStorageFile RemoteFile { get; private set; }
        public Downloader(Windows.Storage.IStorageFile loсalFile, OneDrive.IFileStorageFile file)
        {
            RemoteFile = file;
            _loсalFile = loсalFile;
            _pathToLocalFile = _loсalFile.Path;
            TotalBytesToTransfer = RemoteFile.Size;
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
            DateTime currentTime= DateTime.Now;
            var different= currentTime.Subtract(_lastStep);
            ulong currentDownloaded = _previewDownloadBytes + _downloadOperation.Progress.BytesReceived;
            ulong downloadBytes =currentDownloaded - ByteTransferred;
            ByteTransferred = currentDownloaded;
            if(downloadBytes>0)
                Speed = (downloadBytes / (ulong)different.Milliseconds)*60;
            else
                Speed = 0;

            _lastStep =DateTime.Now;
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
            _loсalFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(_pathToLocalFile);
            var basicProperties= await _loсalFile.GetBasicPropertiesAsync();
            _previewDownloadBytes = (uint)basicProperties.Size;
        }

        private async Task Run()
        {
            string fullPath = AddBackslash(RemoteFile.Path);
            fullPath += RemoteFile.Name;;
          
            BackgroundDownloader downloader = new BackgroundDownloader();
            downloader.SetRequestHeader("Authorization", "Bearer " + RemoteFile.Account.Token);
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
