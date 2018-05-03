using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnityDisk.StorageItems.PreviewImageManager
{
    internal enum CoordinationStatus { AllDone, Timeout, Cancel };

    internal sealed class AsyncCoordinator
    {
        private Int32 m_opCount = 1;
        // Уменьшается на 1 методом AllBegun 
        private Int32 m_statusReported = 0;
        // 0=false, 1=true 
        private Action<CoordinationStatus> m_callback;
        private TaskCompletionSource<CoordinationStatus> _completionSource;
        private Timer m_timer;
        // Этот метод ДОЛЖЕН быть вызван ДО инициирования операции 
        public void AboutToBegin(Int32 opsToAdd = 1)
        {
            Interlocked.Add(ref m_opCount, opsToAdd);
        }
        // Этот метод ДОЛЖЕН быть вызван ПОСЛЕ обработки результата
        public void JustEnded()
        {
            if (Interlocked.Decrement(ref m_opCount) == 0)
                ReportStatus(CoordinationStatus.AllDone);
        }
        // Этот метод ДОЛЖЕН быть вызван ПОСЛЕ инициирования ВСЕХ операций  
        public void AllBegun(Action<CoordinationStatus> callback, Int32 timeout = Timeout.Infinite)
        {
            m_callback = callback;
            if (timeout != Timeout.Infinite)
                m_timer = new Timer(TimeExpired, null, timeout, Timeout.Infinite);
            JustEnded();
        }
        // Этот метод ДОЛЖЕН быть вызван ПОСЛЕ инициирования ВСЕХ операций  
        public Task AllBegun(TaskCompletionSource<CoordinationStatus> callback, Int32 timeout = Timeout.Infinite)
        {
            _completionSource = callback;
            if (timeout != Timeout.Infinite)
                m_timer = new Timer(TimeExpired, null, timeout, Timeout.Infinite);
            JustEnded();
            return callback.Task;
        }

        private void TimeExpired(Object o)
        {
            ReportStatus(CoordinationStatus.Timeout);
        }

        public void Cancel()
        {
            ReportStatus(CoordinationStatus.Cancel);
        }
        private void ReportStatus(CoordinationStatus status)
        {     // Если состояние ни разу не передавалось, передать его; 
            // в противном случае оно игнорируется  
            if (Interlocked.Exchange(ref m_statusReported, 1) == 0)
            {
                _completionSource.SetResult(status);
                m_callback(status);
            }
        }

    }
}
