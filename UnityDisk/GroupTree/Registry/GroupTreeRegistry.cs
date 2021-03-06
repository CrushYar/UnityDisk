﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition.Interactions;
using Unity;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;
using UnityDisk.Settings.Groups;

namespace UnityDisk.GroupTree.Registry
{
    /// <summary>
    /// Реестр групп
    /// </summary>
    public sealed class GroupTreeRegistry : IGroupTreeRegistry
    {
        /// <summary>
        /// Корневой контейнер
        /// </summary>
        private IContainer _groupTree;
        /// <summary>
        /// Настроки
        /// </summary>
        private IGroupSettings _settings;
        /// <summary>
        /// Реест аккаунтов
        /// </summary>
        private IAccountRegistry _accountRegistry;
        /// <summary>
        /// Лок контейнер
        /// </summary>
        private IUnityContainer _unityContainer;

        public event EventHandler<GroupTreeSizeChangedEventArg> ChangedSizeEvent;
        public event EventHandler<GroupTreeActivetyChangedEventArg> ChangedActivetyEvent;
        public event EventHandler<GroupTreeStructureChangedEventArg> ChangedStructureEvent;
        public event EventHandler<GroupTreeItemNameChangedEventArg> ChangedGroupTreeItemNameEvent;

        /// <summary>
        /// Загрузка дерева из настроек
        /// </summary>
        private void LoadSittings()
        {
            _settings = _unityContainer.Resolve<IGroupSettings>();
            _accountRegistry = _unityContainer.Resolve<IAccountRegistry>();

            GroupSettingsContainer root= _settings.LoadGroupTree();

            if (root == null)
            {
                _groupTree= _unityContainer.Resolve<IContainer>();
                _groupTree.Name = "Root";
                _groupTree.Size=new SpaceSize();
                return;
            }

            _groupTree = _unityContainer.Resolve<IContainer>();
            LoadChildren(_groupTree, root);

            _groupTree.LoadSizeInfo();
        }
        /// <summary>
        /// Загрузка дочерних элементов дерева групп
        /// </summary>
        /// <param name="container">Контейнер для загрузки</param>
        /// <param name="settingsContainer">Данные для контейнера</param>
        private void LoadChildren(IContainer container,GroupSettingsContainer settingsContainer)
        {
            container.Name = settingsContainer.Name;
            container.IsActive = settingsContainer.IsActive;
            if(settingsContainer.Items==null)return;

            foreach (var item in settingsContainer.Items)
            {
                switch (item)
                {
                    case GroupSettingsContainer sc:
                        IContainer childContainer = _unityContainer.Resolve<IContainer>();
                        childContainer.Parent = container;
                        LoadChildren(childContainer, sc);
                        container.Items.Add(childContainer);
                        break;
                    case GroupSettingsGroup settingsGroup:
                        IGroup childGroup= _unityContainer.Resolve<IGroup>();
                        childGroup.Name = settingsGroup.Name;
                        childGroup.Parent = container;
                        container.Items.Add(childGroup);
                        if (settingsGroup.Items==null)continue;

                        foreach (var account in settingsGroup.Items)
                        {
                            childGroup.Items.Add(_accountRegistry.Find(account));
                        }
                        break;
                    default:
                        throw new ArgumentException("Unknown type");
                }
            }
        }

        /// <summary>
        /// Сканирует контейнер на наличие элемента
        /// </summary>
        /// <param name="container">Контейнер для анализа</param>
        /// <param name="name">Имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <returns>Возвращает true - при наличии элемента с указанным именем и типом</returns>
        private bool NameScan(IContainer container, string name, GroupTreeTypeEnum type)
        {
            int hashName = name.GetHashCode();
            foreach (var item in container.Items)
            {
                if (item.Type != type) continue;
                if (item.Name.GetHashCode() == hashName) return true;
            }

            return false;
        }

        /// <summary>
        /// Поиск нужной директории (контейнера)
        /// </summary>
        /// <param name="path">Путь по которому нужно произвести поиск</param>
        /// <returns>Найденный контейнер</returns>
        private IContainer FindItemDirectory(IList<string> path)
        {
            string fullCurrentPath = String.Empty;
            IContainer walkerToBottom = _groupTree;

            foreach (var containernNme in path)
            {
                fullCurrentPath += "/" + containernNme;
                walkerToBottom =
                    walkerToBottom.Items.First(it =>
                        it.Name == containernNme && it.Type == GroupTreeTypeEnum.Container) as IContainer;
                if (walkerToBottom == null)
                    throw new InvalidDataException(String.Concat("Don't found the container: '{0}'",
                        fullCurrentPath));
            }

            return walkerToBottom;
        }

        public GroupTreeRegistry()
        {
            _unityContainer = ContainerConfiguration.GetContainer().Container;
        }

        public GroupTreeRegistry(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public void Add(IList<string> path, IGroupTreeItem item)
        {
            string fullCurrentPath = String.Empty;
            IContainer itemDirectory = FindItemDirectory(path);

            if (NameScan(itemDirectory, item.Name, item.Type))
                throw new ArgumentException("Element has already been");

            item.Parent = itemDirectory;
            itemDirectory.Items.Add(item);

            switch (item)
            {
                case IContainer container:
                    _settings.Add(path, new GroupSettingsContainer(container));
                    break;
                case IGroup group:
                    _settings.Add(path, new GroupSettingsGroup(group));
                    break;
                default:
                    throw new ArgumentException("Unknown type");
            }

            SpaceSize oldSize = new SpaceSize(_groupTree.Size);
            _groupTree.LoadSizeInfo();
            SpaceSize newSize = new SpaceSize(_groupTree.Size);

            OnChangedStructureEvent(item, path, RegistryActionEnum.Added);
            OnChangedSizeEvent(oldSize, newSize, GroupTreeSizeChangedEnum.ItemMoved);
        }

        public void Delete(IList<string> path, IGroupTreeItemProjection item)
        {
            Delete(path, item.Name, item.Type);
        }

        public void Delete(IList<string> path, string name, GroupTreeTypeEnum type)
        {
            string fullCurrentPath = String.Empty;
            IContainer itemDirectory = FindItemDirectory(path);

            IGroupTreeItem itemForDelete = itemDirectory.Items.FirstOrDefault(it => it.Name == name && it.Type == type);

            if (itemForDelete == null) throw new DirectoryNotFoundException();

            itemDirectory.Items.Remove(itemForDelete);
            _settings.Delete(path, name, type);

            SpaceSize oldSize = new SpaceSize(_groupTree.Size);
            _groupTree.LoadSizeInfo();
            SpaceSize newSize = new SpaceSize(_groupTree.Size);

            OnChangedStructureEvent(itemForDelete, path, RegistryActionEnum.Removed);
            OnChangedSizeEvent(oldSize, newSize, GroupTreeSizeChangedEnum.ItemDeleted);
        }

        public void Rename(IList<string> path, string oldName, GroupTreeTypeEnum type, string newName)
        {
            string fullCurrentPath = String.Empty;
            IContainer itemDirectory = FindItemDirectory(path);

            IGroupTreeItem itemForRename =
                itemDirectory.Items.FirstOrDefault(it => it.Name == oldName && it.Type == type);

            if (itemForRename == null) throw new DirectoryNotFoundException();

            if (NameScan(itemDirectory, newName, type))
                throw new ArgumentException("Element has already been");

            itemForRename.Name = newName;

            _settings.Rename(path, oldName, type, newName);

            OnChangedGroupTreeItemNameEvent(itemForRename, path, oldName, newName);
        }

        public void Rename(IList<string> path, IGroupTreeItemProjection item, string newName)
        {
            Rename(path, item.Name, item.Type, newName);
        }

        public void Move(IList<string> path, string name, GroupTreeTypeEnum type, IList<string> newPath)
        {
            string fullCurrentPath = String.Empty;
            IContainer itemDirectoryFrom = FindItemDirectory(path);
            IContainer itemDirectoryTo = FindItemDirectory(newPath);

            IGroupTreeItem itemForMove =
                itemDirectoryFrom.Items.FirstOrDefault(it => it.Name == name && it.Type == type);

            if (itemForMove == null) throw new DirectoryNotFoundException();

            if (NameScan(itemDirectoryTo, itemForMove.Name, itemForMove.Type))
                throw new ArgumentException("Element has already been");

            itemDirectoryTo.Items.Add(itemForMove);

            itemForMove.Parent = itemDirectoryTo;
            itemDirectoryFrom.Items.Remove(itemForMove);

            _settings.Move(path, name, type, newPath);

            SpaceSize oldSize=new SpaceSize(_groupTree.Size);
            _groupTree.LoadSizeInfo();
            SpaceSize newSize = new SpaceSize(_groupTree.Size);

            OnChangedStructureEvent(itemForMove, newPath, RegistryActionEnum.Added);
            OnChangedStructureEvent(itemForMove, path, RegistryActionEnum.Removed);
            OnChangedSizeEvent(oldSize, newSize, GroupTreeSizeChangedEnum.ItemMoved);
        }

        public void Move(IList<string> path, IGroupTreeItemProjection item, IList<string> newPath)
        {
            Move(path, item.Name, item.Type, newPath);
        }

        public void Copy(IList<string> path, IGroupTreeItemProjection item, IList<string> newPath)
        {
            Copy(path, item.Name, item.Type, newPath);
        }

        public void Copy(IList<string> path, string name, GroupTreeTypeEnum type, IList<string> otherPath)
        {
            string fullCurrentPath = String.Empty;
            IContainer itemDirectoryFrom = FindItemDirectory(path);
            IContainer itemDirectoryTo = FindItemDirectory(otherPath);

            IGroupTreeItem itemForCopy =
                itemDirectoryFrom.Items.FirstOrDefault(it => it.Name == name && it.Type == type);

            if (itemForCopy == null) throw new DirectoryNotFoundException();

            if (NameScan(itemDirectoryTo, itemForCopy.Name, itemForCopy.Type))
                throw new ArgumentException("Element has already been");

            IGroupTreeItem newItem = itemForCopy.Clone();

            newItem.Parent = itemDirectoryTo;
            itemDirectoryTo.Items.Add(newItem);

            _settings.Copy(path, name, type, otherPath);

            SpaceSize oldSize = new SpaceSize(_groupTree.Size);
            _groupTree.LoadSizeInfo();
            SpaceSize newSize = new SpaceSize(_groupTree.Size);

            OnChangedStructureEvent(newItem, otherPath, RegistryActionEnum.Added);
            OnChangedSizeEvent(oldSize, newSize, GroupTreeSizeChangedEnum.ItemCopied);

        }

        public void SetActive(IList<string> path, string name, bool value)
        {
            string fullCurrentPath = String.Empty;
            IContainer itemDirectory = FindItemDirectory(path);

            IContainer itemForSetActive =
                itemDirectory.Items.FirstOrDefault(it => it.Name == name && it.Type == GroupTreeTypeEnum.Container) as
                    IContainer;

            if (itemForSetActive == null) throw new DirectoryNotFoundException();

            bool oldValue = itemForSetActive.IsActive;
            itemForSetActive.IsActive = value;

            _settings.SetActive(path, name, value);

            OnChangedActivetyEvent(itemForSetActive, oldValue, value, path);
        }

        public void Initialization()
        {
            LoadSittings();
        }
        public IContainerProjection GetContainerProjection(IList<string> path,string name)
        {
            string fullCurrentPath = String.Empty;
            IContainer itemDirectory = FindItemDirectory(path);

            if(String.IsNullOrEmpty(name)) return  new ContainerProjection(itemDirectory);

            IContainer container =
                itemDirectory.Items.FirstOrDefault(it => it.Name == name && it.Type == GroupTreeTypeEnum.Container) as
                    IContainer;

            if (container == null) throw new DirectoryNotFoundException();

            return new ContainerProjection(container);
        }

        void OnChangedStructureEvent(IGroupTreeItem item, IList<string> path, RegistryActionEnum action)
        {
            ChangedStructureEvent?.Invoke(this, new GroupTreeStructureChangedEventArg(item, action) {Path = path});
        }

        void OnChangedSizeEvent(SpaceSize oldSize, SpaceSize newSize,
            GroupTreeSizeChangedEnum action)
        {
            ChangedSizeEvent?.Invoke(this,
                new GroupTreeSizeChangedEventArg(oldSize, newSize, action));
        }

        void OnChangedGroupTreeItemNameEvent(IGroupTreeItem item, IList<string> path, string oldName, string newName)
        {
            ChangedGroupTreeItemNameEvent?.Invoke(this,
                new GroupTreeItemNameChangedEventArg(item, oldName, newName) {Path = path});
        }

        void OnChangedActivetyEvent(IContainer container, bool oldValue, bool newValue, IList<string> path)
        {
            ChangedActivetyEvent?.Invoke(this,
                new GroupTreeActivetyChangedEventArg(container, oldValue, newValue) {Path = path});
        }
    }
}
