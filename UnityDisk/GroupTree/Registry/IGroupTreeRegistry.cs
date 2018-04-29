using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.GroupTree.Registry
{
    /// <summary>
    /// Базовый интерфейс реестра дерева групп
    /// </summary>
   public interface IGroupTreeRegistry
    {
        /// <summary>
        /// Событие при изменении размера одного из элементов дерева групп
        /// </summary>
        event EventHandler<GroupTreeSizeChangedEventArg> ChangedSizeEvent;
        /// <summary>
        /// Событие при изменении состояния активности одного из контейнеров дерева групп
        /// </summary>
        event EventHandler<GroupTreeActivetyChangedEventArg> ChangedActivetyEvent;
        /// <summary>
        /// Событие при изменении структуры дерева групп
        /// </summary>
        event EventHandler<GroupTreeStructureChangedEventArg> ChangedStructureEvent;
        /// <summary>
        /// Событие при изменении имени элемента дерева групп
        /// </summary>
        event EventHandler<GroupTreeItemNameChangedEventArg> ChangedGroupTreeItemNameEvent;
        /// <summary>
        /// Добавление нового элемента по указанному пути
        /// </summary>
        /// <param name="path">Пусть, относительно которого нужно добавить группу</param>
        /// <param name="item">Добавляемый элемент</param>
        void Add(IList<string> path, IGroupTreeItem item);

        /// <summary>
        /// Удаление элемента по указанному пути
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="name">Имя элемента</param>
        /// <param name="type">Тип элемента</param>
        void Delete(IList<string> path,string name, GroupTreeTypeEnum type);
        /// <summary>
        /// Удаление элемента по указанному пути
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="item">Удаляемый элемент</param>
        void Delete(IList<string> path, IGroupTreeItemProjection item);
      
        /// <summary>
        /// Переименование элемента
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="oldName">Старое имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <param name="newName">Новое имя элемента</param>
       void Rename(IList<string> path, string oldName, GroupTreeTypeEnum type, string newName);
        /// <summary>
        /// Переименование элемента
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="item">Элемент для переименования</param>
        /// <param name="newName">Новое имя элемента</param>
        void Rename(IList<string> path, IGroupTreeItemProjection item, string newName);
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="name">Имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <param name="newPath">Целевой путь, не включая имя</param>
        void Move(IList<string> path,string name, GroupTreeTypeEnum type, IList<string> newPath);
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="item">Элемент для перемещения</param>
        /// <param name="newPath">Целевой путь</param>
        void Move(IList<string> path, IGroupTreeItemProjection item, IList<string> newPath);
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="item">Элемент для копирования</param>
        /// <param name="newPath">Целевой путь</param>
        void Copy(IList<string> path, IGroupTreeItemProjection item, IList<string> newPath);
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="name">Имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <param name="otherPath">Целевой путь</param>
        void Copy(IList<string> path, string name,GroupTreeTypeEnum type, IList<string> otherPath);
        /// <summary>
        /// Изменение состояния контейнера
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="name">Имя элемента</param>
        /// <param name="value">Значение</param>
        void SetActive(IList<string> path, string name, bool value);
        /// <summary>
        /// Загрузка данных
        /// </summary>
        void Initialization();
    }
}
