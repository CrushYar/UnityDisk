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
       /// Добавление новой группы по указанному пути
       /// </summary>
       /// <param name="path">Пусть, относительно которого нужно добавить группу</param>
       /// <param name="group">Добавляемая группа</param>
       void Add(Queue<string> path, IGroup group);
        /// <summary>
        /// Добавление нового контейнера по указанному пути
        /// </summary>
        /// <param name="path">Пусть, относительно которого нужно добавить группу</param>
        /// <param name="container">Добавляемый контейнер</param>
        void Add(Queue<string> path, IContainer container);
        /// <summary>
        /// Удаление элемента по указанному пути
        /// </summary>
        /// <param name="fullPath">Полный путь к элементу включая имя элемента</param>
        /// <param name="type">Тип элемента</param>
       void Delete(Queue<string> fullPath, GroupTreeTypeEnum type);
        /// <summary>
        /// Удаление группы по указанному пути
        /// </summary>
        /// <param name="path">Полный путь к группе</param>
        /// <param name="group">Удаляемая группа</param>
        void Delete(Queue<string> path, IGroup group);
       /// <summary>
       /// Удаление контейнера по указанному пути
       /// </summary>
       /// <param name="path">Полный путь к контейнеру</param>
       /// <param name="container">Удаляемый контейнер</param>
       void Delete(Queue<string> path, IContainer container);
        /// <summary>
        /// Переименование элемента
        /// </summary>
        /// <param name="path">Путь к элементу, не включая имя элемента</param>
        /// <param name="oldName">Старое имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <param name="newName">Новое имя элемента</param>
       void Rename(Queue<string> path, string oldName, GroupTreeTypeEnum type, string newName);
        /// <summary>
        /// Переименование группы
        /// </summary>
        /// <param name="path">Путь к группе</param>
       /// <param name="group">Группа для переименования</param>
        /// <param name="newName">Новое имя группы</param>
        void Rename(Queue<string> path, IGroup group,string newName);
       /// <summary>
       /// Переименование контейнера
       /// </summary>
       /// <param name="path">Путь к контейнеру</param>
       /// <param name="container">Контейнер для переименования</param>
       /// <param name="newName">Новое имя контейнера</param>
       void Rename(Queue<string> path, IContainer container, string newName);
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="oldFullPath">Полный путь к элементу, включая имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <param name="newPath">Целевой путь, не включая имя</param>
        void Move(Queue<string> oldFullPath, GroupTreeTypeEnum type, Queue<string> newPath);
       /// <summary>
       /// Перемещение группы
       /// </summary>
       /// <param name="path">Путь к группе</param>
       /// <param name="group">Группа для перемещения</param>
       /// <param name="newPath">Целевой путь</param>
       void Move(Queue<string> path, IGroup group, Queue<string> newPath);
       /// <summary>
       /// Перемещаемая группа
       /// </summary>
       /// <param name="path">Путь к контейнеру</param>
       /// <param name="container">Контейнер для перемещения</param>
       /// <param name="newPath">Целевой путь</param>
       void Move(Queue<string> path, IContainer container, Queue<string> newPath);
        /// <summary>
        /// Копирование группы
        /// </summary>
        /// <param name="path">Путь к группе</param>
        /// <param name="group">Группа для копирования</param>
        /// <param name="newPath">Целевой путь</param>
       void Copy(Queue<string> path, IGroup group, Queue<string> newPath);
        /// <summary>
        /// Копирование контейнера
        /// </summary>
        /// <param name="path">Путь к контейнеру</param>
        /// <param name="container">Контейнер для копирования</param>
        /// <param name="newPath">Целевой путь</param>
        void Copy(Queue<string> path, IContainer container, Queue<string> newPath);
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="fullPath">Полный путь к элементу, включая имя</param>
        /// <param name="type">Тип элемента</param>
        /// <param name="otherPath">Целевой путь</param>
        void Copy(Queue<string> fullPath, GroupTreeTypeEnum type, Queue<string> otherPath);
    }
}
