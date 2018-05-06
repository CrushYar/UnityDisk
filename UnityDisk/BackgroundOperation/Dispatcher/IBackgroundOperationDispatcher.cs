using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.BackgroundOperation.Dispatcher
{
    /// <summary>
    /// Диспетчер операций загрузки и скачивания
    /// </summary>
    public interface IBackgroundOperationDispatcher
    {
        IList<IBackgroundOperation> Operations { get; }
        event EventHandler<UploadedEventArg> UploadedEvent;
        event EventHandler<DownloadedEventArg> DownloadedEvent;
        void Initialization();
        void Registry(IBackgroundOperation operation);
    }
}
