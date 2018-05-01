using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;

namespace UnityDisk.GroupTree
{
    public enum GroupTreeTypeEnum
    {
        Group,Container
    }
    /// <summary>
    /// Базовый интерфейс элементов
    /// </summary>
   public interface IGroupTreeItem : ICloneable<IGroupTreeItem>
    {
        /// <summary>
        /// Название элемента группы
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Размер элемента
        /// </summary>
        SpaceSize Size { get; set; }
        /// <summary>
        /// Родитель
        /// </summary>
        IContainer Parent { get; set; }
        /// <summary>
        /// Тип элемента
        /// </summary>
        GroupTreeTypeEnum Type { get; }
        /// <summary>
        /// Загрузка информации об размере
        /// </summary>
        void LoadSizeInfo();
        /// <summary>
        /// Загрузка корневой директории всех аккаунтов ниже по дереву
        /// </summary>
        void /*IList<IStorageItem>*/ LoadDirectory();
        /// <summary>
        /// Получение коллекции аккаунтов находящийся внутри элемента
        /// </summary>
        /// <returns>Список аккаунтов</returns>
        List<IAccountProjection> GetAccountProjections();
    }
}
