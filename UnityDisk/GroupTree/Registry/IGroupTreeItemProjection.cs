using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;

namespace UnityDisk.GroupTree.Registry
{
   public interface IGroupTreeItemProjection:ICloneable<IGroupTreeItemProjection>
    {
        /// <summary>
        /// Название элемента
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Размер элемента
        /// </summary>
        SpaceSize Size { get; }
        /// <summary>
        /// Тип элемента
        /// </summary>
        GroupTreeTypeEnum Type { get; }
        /// <summary>
        /// Получение коллекции аккаунтов находящийся внутри элемента
        /// </summary>
        /// <returns>Список аккаунтов</returns>
        List<IAccountProjection> GetAccountProjections();
        /// <summary>
        /// Загрузка корневой директории всех аккаунтов ниже по дереву
        /// </summary>
        Task<FileStorages.IFileStorageFolder> LoadDirectory();
        /// <summary>
        /// Проверка элементов на равенство (использовать только для тестов)
        /// </summary>
        /// <param name="projection">Элемент с которым нужно сравнить</param>
        /// <returns>Результат равенства</returns>
        bool Equals(IGroupTreeItemProjection projection);
    }
}
