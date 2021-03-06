﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.FileStorages;

namespace UnityDisk.StorageItems
{
    /// <summary>
    /// Интерфейс проекции папок
    /// </summary>
    public interface IStorageProjectionFolder:IStorageFolder
    {
        /// <summary>
        /// Спроектированные папки
        /// </summary>
        IList<IStorageFolder2> Folders { get; }
        /// <summary>
        /// Удаление папок
        /// </summary>
        /// <param name="folders">Папки для удаления</param>
        Task Delete(IList<IStorageFolder2> folders);
        /// <summary>
        /// Переименование
        /// </summary>
        /// <param name="folders">Папки для переименования</param>
        Task Rename(IList<IStorageFolder2> folders, string newName);
        /// <summary>
        /// Перемещение
        /// </summary>
        /// <param name="folders">Папки для перемещения</param>
        /// <param name="folder">Целевая папка</param>
        Task Move(IList<IStorageFolder2> folders, IStorageProjectionFolder folder);
        /// <summary>
        /// Копирование
        /// </summary>
        /// <param name="folders">Папки для копирования</param>
        /// <param name="folder">Целевая папка</param>
        Task Copy(IList<IStorageFolder2> folders, IStorageProjectionFolder folder);
        /// <summary>
        /// Создание публичных ссылок
        /// </summary>
        /// <param name="folders">Папки для которых нужно создать публичную ссылку</param>
        Task CreatePublicUrl(IList<IStorageFolder2> folders);
        /// <summary>
        /// Загрузка публичных ссылок
        /// </summary>
        /// <param name="folders">Папки для которых нужно загрузить публичные ссылки</param>
        Task LoadPublicUrl(IList<IStorageFolder2> folders);
        /// <summary>
        /// Удаление публичных ссылок
        /// </summary>
        /// <param name="folders">Папки на которые нужно удалить публичные ссылки</param>
        Task DeletePublicUrl(IList<IStorageFolder2> folders);

    }
}
