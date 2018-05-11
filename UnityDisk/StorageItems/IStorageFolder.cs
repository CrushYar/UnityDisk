using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using UnityDisk.BackgroundOperation;
using UnityDisk.FileStorages;

namespace UnityDisk.StorageItems
{
    /// <summary>
    /// Интерфейст дикертории файловой системы
    /// </summary>
    public interface IStorageFolder:IStorageItem
    {
        /// <summary>
        /// Загрузка директории
        /// </summary>
        /// <returns>Загруженные элементы</returns>
        Task<List<IStorageItem>> LoadDirectory();

        /// <summary>
        /// Загрузка файла в директорию облака
        /// </summary>
        /// <param name="storageFile">Загружаемый файл</param>
        Task<BackgroundOperation.IUploader> Upload(Windows.Storage.IStorageFile storageFile);
        /// <summary>
        /// Создание новой папки
        /// </summary>
        /// <param name="name">Имя папки</param>
        /// <returns></returns>
        Task <IStorageFolder> CreateFolder(string name);
        /// <summary>
        /// Делает проэкцию папки во всех аккаунтах
        /// </summary>
        Task MakeProjection();
    }
}
