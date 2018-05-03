using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.FileStorages;

namespace UnityDisk.StorageItems
{
    /// <summary>
    /// Интерфейс связывающий два других интерфейса для логического разделения файлов, папок и проекций папок
    /// </summary>
    public interface IStorageFolder2:IStorageItem2,IStorageFolder
    {
        /// <summary>
        /// Контекс данных
        /// </summary>
        IFileStorageFolder DataContext { get; set; }
        /// <summary>
        /// Создание новой папки
        /// </summary>
        /// <param name="name">Имя папки</param>
        /// <returns></returns>
        Task<IStorageFolder2> CreateFolder2(string name);
    }
}
