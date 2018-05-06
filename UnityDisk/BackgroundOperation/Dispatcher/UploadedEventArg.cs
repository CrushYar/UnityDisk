using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.BackgroundOperation.Dispatcher
{
    public class UploadedEventArg:EventArgs
    {
        public UploadedEventArg(StorageItems.IStorageFile storageFile, IList<string> path)
        {
            StorageFile = storageFile;
            Path = path;
        }
        /// <summary>
        /// Загруженный файл
        /// </summary>
        public UnityDisk.StorageItems.IStorageFile StorageFile { get; private set; }
        /// <summary>
        /// Путь месту загрузки
        /// </summary>
        public IList<string> Path { get; private set; }
    }
}
