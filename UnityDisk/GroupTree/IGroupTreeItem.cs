using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.GroupTree
{
    /// <summary>
    /// Базовый интерфейс элементов групп
    /// </summary>
   public interface IGroupTreeItem
    {
        /// <summary>
        /// Название элемента группы
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Размер элемента группы
        /// </summary>
        SpaceSize Size { get; }
        /// <summary>
        /// Загрузка информации об размере
        /// </summary>
        void LoadSizeInfo();
        /// <summary>
        /// Загрузка корневой директории всех аккаунтов ниже по дереву
        /// </summary>
        void /*IList<IStorageItem>*/ LoadDirectory();
    }
}
