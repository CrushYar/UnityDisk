using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.GroupTree;

namespace UnityDisk.Settings.Groups
{
    public interface IGroupSettings
    {
        /// <summary>
        /// Загрузка дерева групп
        /// </summary>
        /// <returns></returns>
        GroupSettingsContainer LoadGroupTree();
        /// <summary>
        /// Сохранение дерева групп
        /// </summary>
        void SaveGroupTree();
        /// <summary>
        /// Добавление элемента по указанному пути
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="item">Добавляемый элемент</param>
        void Add(IList<string> path, GroupSettingsItem item);
        /// <summary>
        /// Удаление элемента по указанному пути
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="name">Имя элемента</param>
        /// <param name="type">Тип элемента</param>
        void Delete(IList<string> path, string name, GroupTreeTypeEnum type);
        /// <summary>
        /// Переименование элемента
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="oldName">Старое имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <param name="newName">Новое имя элемента</param>
        void Rename(IList<string> path, string oldName, GroupTreeTypeEnum type, string newName);
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="name">Имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <param name="newPath">Целевой путь, не включая имя</param>
        void Move(IList<string> path, string name, GroupTreeTypeEnum type, IList<string> newPath);
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="name">Имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <param name="otherPath">Целевой путь</param>
        void Copy(IList<string> path, string name, GroupTreeTypeEnum type, IList<string> otherPath);
        /// <summary>
        /// Изменение состояния контейнера
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="name">Имя элемента</param>
        /// <param name="value">Значение</param>
        void SetActive(IList<string> path, string name, bool value);

    }
}
