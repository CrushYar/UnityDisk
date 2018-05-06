using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityDisk.StorageItems.PreviewImageManager;

namespace UnityDisk.BackgroundOperation.Dispatcher
{
    public class BackgroundOperationDispatcher:IBackgroundOperationDispatcher
    {
        private IList<IBackgroundOperation> _inQueue;
        private IList<IBackgroundOperation> _inWait;
        private IList<IBackgroundOperation> _inProccess;
        private IList<IBackgroundOperation> _history;
        private SpinLock _spinLockQueue = new SpinLock(true);
        private SpinLock _spinLockWait = new SpinLock(true);
        private SpinLock _spinLockProccess = new SpinLock(true);
        private SpinLock _spinLockHistory = new SpinLock(true);
        private bool IsDispatcherRuned;
        private AsyncCoordinator m_ac = new AsyncCoordinator();

        public IList<IBackgroundOperation> Operations { get; private set; }
        public event EventHandler<UploadedEventArg> UploadedEvent;
        public event EventHandler<DownloadedEventArg> DownloadedEvent;
        public void Initialization()
        {
            throw new NotImplementedException();
        }

        public void Registry(IBackgroundOperation operation)
        {
            LockQueue();
            _inQueue.Add(operation);

            if (!IsDispatcherRuned)
            {
                IsDispatcherRuned = true;
                Task.Run(RunDispatcher);
            }
            UnLockQueue();
        }

        private async Task RunDispatcher()
        {
            int activeLoaderCount = 2;

            while (true)
            {
                LockQueue();
                foreach (var item in _inQueue)
                {
                    _inWait.Add(item);
                }
                if (_inQueue.Count == 0)
                {
                    IsDispatcherRuned = false;
                    return;
                }
                UnLockQueue();

                do
                {
                    LockWait();
                    LockProccess();
                    for (int i = 0; i < activeLoaderCount && _inWait.Count > 0; i++)
                    {
                        _inProccess.Add(_inWait[0]);
                        _inWait.RemoveAt(0);
                    }
                    if (_inProccess.Count == 0)
                        break;

                    UnLockProccess();
                    UnLockWait();

                    m_ac.AboutToBegin(activeLoaderCount);

                    foreach (var item in _inProccess)
                    {
                        await item.Start().ContinueWith(task => m_ac.JustEnded());
                    }

                    var token = new TaskCompletionSource<CoordinationStatus>();
                    await m_ac.AllBegun(token);

                    foreach (var itemDone in _inProccess)
                    {
                        LockHistory();
                        _history.Add(itemDone);
                        UnLockHistory();
                    }
                } while (true);
            }
        }

        private void SaveState() { }
        private void LoadState() { }

        private void OnUploadedEvent(UnityDisk.StorageItems.IStorageFile storageFile, IList<string> path)
        {
            UploadedEvent?.Invoke(this, new UploadedEventArg(storageFile, path));
        }

        private void OnDownloadedEvent(UnityDisk.StorageItems.IStorageFile storageFile, IList<string> path)
        {
            DownloadedEvent?.Invoke(this, new DownloadedEventArg(storageFile, path));
        }


        private void LockQueue()
        {
            bool taken = false;
            _spinLockQueue.Enter(ref taken);
        }
        private void UnLockQueue()
        {
            _spinLockQueue.Exit();
        }
        private void LockWait()
        {
            bool taken = false;
            _spinLockWait.Enter(ref taken);
        }
        private void UnLockWait()
        {
            _spinLockQueue.Exit();
        }
        private void LockProccess()
        {
            bool taken = false;
            _spinLockProccess.Enter(ref taken);
        }
        private void UnLockProccess()
        {
            _spinLockProccess.Exit();
        }
        private void LockHistory()
        {
            bool taken = false;
            _spinLockHistory.Enter(ref taken);
        }
        private void UnLockHistory()
        {
            _spinLockHistory.Exit();
        }
    }
}
