using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.BackgroundOperation.Dispatcher
{
    public class DownloadedEventArg:EventArgs
    {
        public DownloadedEventArg(StorageItems.IStorageFile storageFile, IList<string> path)
        {
            StorageFile = storageFile;
            Path = path;
        }
        /// <summary>
        /// Скачанный файл
        /// </summary>
        public UnityDisk.StorageItems.IStorageFile StorageFile { get; private set; }
        /// <summary>
        /// Путь скачанного файла
        /// </summary>
        public IList<string> Path { get; private set; }
    }
}
