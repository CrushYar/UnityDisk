using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages
{
    /// <summary>
    /// Интерфейс файла облачного сервиса
    /// </summary>
    public interface IFileStorageFile:IFileStorageItem
    {
        /// <summary>
        /// Размер в байтах
        /// </summary>
        ulong Size { get; }
        /// <summary>
        /// Зкачивание файла
        /// </summary>
        /// <param name="file">Локальный файл, куда буду записаны байты</param>
        /// <returns>Объект процесса</returns>
        BackgroundOperation.IDownloader Download(Windows.Storage.IStorageFile file);
    }
}
