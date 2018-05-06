using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnityDisk.StorageItems.PreviewImageManager
{
    public class LoadPreviewImageManager : ILoadPreviewImageManager
    {
        private Queue<IStorageItem> _inQueue = new Queue<IStorageItem>();
        private Queue<IStorageItem> _inProccess = new Queue<IStorageItem>();
        private bool isRun;
        // Этот класс Helper координирует все асинхронные операции  
        private AsyncCoordinator m_ac = new AsyncCoordinator();

        /// <summary>
        /// Объект синхронизации
        /// </summary>
        private SpinLock _spinLock = new SpinLock(true);

        public void LoadPreviewImages(IList<IStorageItem> items)
        {
            Lock();
            foreach (var item in items)
            {
                _inQueue.Enqueue(item);
            }

            if (!isRun)
            {
                isRun = true;
                Task.Run(async () =>await Manager());
            }

            UnLock();
        }

        public void Reset()
        {
            Lock();
            isRun = false;
            _inQueue.Clear();
            _inProccess.Clear();
            UnLock();
        }

        private async Task Manager()
        {
            int activeLoaderCount = 2;

            while (true)
            {
                Lock();
                foreach (var item in _inQueue)
                {
                    _inProccess.Enqueue(item);
                }

                if (_inProccess.Count == 0)
                {
                    isRun = false;
                    return;
                }

                UnLock();

                int loadNumber = 0;
                m_ac.AboutToBegin(activeLoaderCount);
                try
                {
                    foreach (var item in _inProccess)
                    {
                        loadNumber++;

                        await item.LoadPublicUrl().ContinueWith(task => m_ac.JustEnded());
                        if (loadNumber == activeLoaderCount)
                        {
                            loadNumber = 0;

                            var token = new TaskCompletionSource<CoordinationStatus>();
                            await m_ac.AllBegun(token);
                        }
                        if(!isRun)return;
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        private void Lock()
        {
            bool taken = false;
            _spinLock.Enter(ref taken);
        }

        private void UnLock()
        {
            _spinLock.Exit();
        }
    }
}
