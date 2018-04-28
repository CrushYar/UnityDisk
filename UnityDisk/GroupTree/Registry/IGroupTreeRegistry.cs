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
        /// Событие при изменении структуры дерева групп
        /// </summary>
        event EventHandler<GroupTreeStructureChangedEventArg> ChangedStructureEvent;
        /// <summary>
        /// Добавление нового элемента по указанному пути
        /// </summary>
        /// <param name="path">Пусть, относительно которого нужно добавить группу</param>
        /// <param name="item">Добавляемый элемент</param>
        void Add(Queue<string> path, IGroupTreeItem item);
       
        /// <summary>
        /// Удаление элемента по указанному пути
        /// </summary>
        /// <param name="fullPath">Полный путь к элементу включая имя элемента</param>
        /// <param name="type">Тип элемента</param>
       void Delete(Queue<string> fullPath, GroupTreeTypeEnum type);
        /// <summary>
        /// Удаление элемента по указанному пути
        /// </summary>
        /// <param name="path">Полный путь к элементу</param>
        /// <param name="item">Удаляемый элемент</param>
        void Delete(Queue<string> path, IGroupTreeItem item);
      
        /// <summary>
        /// Переименование элемента
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="oldName">Старое имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <param name="newName">Новое имя элемента</param>
       void Rename(Queue<string> path, string oldName, GroupTreeTypeEnum type, string newName);
        /// <summary>
        /// Переименование элемента
        /// </summary>
        /// <param name="path">Путь к элементу</param>
        /// <param name="item">Элемент для переименования</param>
        /// <param name="newName">Новое имя элемента</param>
        void Rename(Queue<string> path, IGroupTreeItem item, string newName);
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="oldFullPath">Полный путь к элементу, включая имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <param name="newPath">Целевой путь, не включая имя</param>
        void Move(Queue<string> oldFullPath, GroupTreeTypeEnum type, Queue<string> newPath);
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="path">Путь к элементу</param>
        /// <param name="item">Элемент для перемещения</param>
        /// <param name="newPath">Целевой путь</param>
        void Move(Queue<string> path, IGroupTreeItem item, Queue<string> newPath);
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="path">Путь к элементу</param>
        /// <param name="item">Элемент для копирования</param>
        /// <param name="newPath">Целевой путь</param>
        void Copy(Queue<string> path, IGroupTreeItem item, Queue<string> newPath);
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="fullPath">Полный путь к элементу, включая имя</param>
        /// <param name="type">Тип элемента</param>
        /// <param name="otherPath">Целевой путь</param>
        void Copy(Queue<string> fullPath, GroupTreeTypeEnum type, Queue<string> otherPath);
        /// <summary>
        /// Загрузка данных
        /// </summary>
        void Load();
    }
}
