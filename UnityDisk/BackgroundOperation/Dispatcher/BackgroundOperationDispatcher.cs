using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;
using UnityDisk.Settings.BackgroundOperations;
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
        private bool _isDispatcherRuned;
        private AsyncCoordinator _mAc = new AsyncCoordinator();
        private readonly Settings.BackgroundOperations.IBackgroundOperationDispatcherSettings _settings;

        public IList<IBackgroundOperation> Operations { get; private set; }
        public event EventHandler<UploadedEventArg> UploadedEvent;
        public event EventHandler<DownloadedEventArg> DownloadedEvent;

        public BackgroundOperationDispatcher()
        {
            IUnityContainer container = ContainerConfiguration.GetContainer().Container;
            _settings= container.Resolve<IBackgroundOperationDispatcherSettings>();
        }
        public void Initialization()
        {
          LoadState();
        }

        public void Registry(IBackgroundOperation operation)
        {
            LockQueue();
            _inQueue.Add(operation);

            if (!_isDispatcherRuned)
            {
                _isDispatcherRuned = true;
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
                    _isDispatcherRuned = false;
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

                    _mAc.AboutToBegin(activeLoaderCount);

                    foreach (var item in _inProccess)
                    {
                        await item.Start().ContinueWith(task => _mAc.JustEnded());
                    }

                    var token = new TaskCompletionSource<CoordinationStatus>();
                    await _mAc.AllBegun(token);

                    foreach (var itemDone in _inProccess)
                    {
                        LockHistory();
                        _history.Add(itemDone);
                        UnLockHistory();
                    }
                } while (true);
            }
        }

        private void SaveState()
        {
            LockQueue();
            LockWait();
            LockProccess();
            LockHistory();

            _settings.SaveOperations("BO_inQueue",_inQueue);
            _settings.SaveOperations("BO_inWait",_inWait);
            _settings.SaveOperations("BO_inProccess",_inProccess);
            _settings.SaveOperations("BO_history",_history);

            UnLockHistory();
            UnLockProccess();
            UnLockWait();
            UnLockQueue();
        }
        private void LoadState()
        {
            _inQueue = _settings.LoadOperations("BO_inQueue");
            _inWait = _settings.LoadOperations("BO_inWait");
            _inProccess = _settings.LoadOperations("BO_inProccess");
            _history = _settings.LoadOperations("BO_history");
        }

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
