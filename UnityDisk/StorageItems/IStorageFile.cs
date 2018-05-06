using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using UnityDisk.FileStorages;

namespace UnityDisk.StorageItems
{
    /// <summary>
    /// Интерфейс файла файловой системы
    /// </summary>
    public interface IStorageFile:IStorageItem,IStorageItem2
    {
        /// <summary>
        /// Размер элемента в байтах
        /// </summary>
        ulong Size { get; }
        /// <summary>
        /// Тип элемента
        /// </summary>
        string Type { get; }
        /// <summary>
        /// Скачивание элемента
        /// </summary>
        /// <param name="storageFile">Файл в файловой системе компьютера, куда запишутся все байты</param>
        Task Download(Windows.Storage.IStorageFile storageFile);
        /// <summary>
        /// Контекс данных
        /// </summary>
        IFileStorageFile DataContext { get; set; }
        /// <summary>
        /// Импортирует данные из строки
        /// </summary>
        /// <param name="data">Данные в строковом виде</param>
        void Parse(String data);
        /// <summary>
        /// Получение данных в строковом виде
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}
