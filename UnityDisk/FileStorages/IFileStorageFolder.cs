using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages
{
    /// <summary>
    /// Интерфейс папки облачного сервиса
    /// </summary>
    public interface IFileStorageFolder:IFileStorageItem
    {
        /// <summary>
        /// Загрузка директории
        /// </summary>
        /// <returns>Коллекция загруженных элементов</returns>
        IList<IFileStorageItem> LoadDirectory();
        /// <summary>
        /// Загрузка локального файла в облако
        /// </summary>
        /// <param name="loacalFile">Локальный файл</param>
        void Upload(Windows.Storage.IStorageFile loacalFile);
        /// <summary>
        /// Создание папки
        /// </summary>
        /// <param name="name">Имя папки</param>
        /// <returns></returns>
        IFileStorageFolder CreateFolder(string name);
    }
}
