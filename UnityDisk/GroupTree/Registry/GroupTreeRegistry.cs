using System;
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
    public sealed class GroupTreeRegistry:IGroupTreeRegistry
    {
        private IContainer _groupTree;
        private IGroupSettings _settings;
        private IUnityContainer _container;

        public event EventHandler<GroupTreeSizeChangedEventArg> ChangedSizeEvent;
        public event EventHandler<GroupTreeStructureChangedEventArg> ChangedStructureEvent;
        public event EventHandler<GroupTreeItemNameChangedEventArg> ChangedGroupTreeItemNameEvent;

        /// <summary>
        /// Загрузка дерева из настроек
        /// </summary>
        private void LoadSittings()
        {
            _container = ContainerConfiguration.GetContainer().Container;
            _settings = _container.Resolve<IGroupSettings>();
        }

        /// <summary>
        /// Сохранение дерева в настройках
        /// </summary>
        private void SaveSittings()
        {
            _settings.SaveGroupTree();
        }

        /// <summary>
        /// Сканирует контейнер на наличие элемента
        /// </summary>
        /// <param name="container">Контейнер для анализа</param>
        /// <param name="name">Имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <returns>Возвращает true - при наличии элемента с указанным именем и типом</returns>
        private bool NameScan(IContainer container,string name,GroupTreeTypeEnum type)
        {
            int hashName = name.GetHashCode();
            foreach (var item in container.Items)
            {
                if (item.Type != type) continue;
                if (item.Name.GetHashCode() == hashName) return true;
            }

            return false;
        }

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

        public void Add(IList<string> path, IGroupTreeItem item)
        {
            List<string> copyPath = new List<string>(path);

            string fullCurrentPath = String.Empty;
            IContainer itemDirectory = FindItemDirectory(path);

            if (NameScan(itemDirectory, item.Name,item.Type))
                throw new ArgumentException("Element has already been");

            item.Parent = itemDirectory;
            itemDirectory.Items.Add(item);

            IContainer walkerToTop= itemDirectory;
            SpaceSize oldSize = new SpaceSize(walkerToTop.Size);
            walkerToTop.Size.Addition(item.Size);
            SpaceSize newSize = new SpaceSize(walkerToTop.Size);
            walkerToTop = walkerToTop.Parent;

            while (walkerToTop != null)
            {
                walkerToTop.Size.Addition(item.Size);
                walkerToTop = walkerToTop.Parent;
            }

            SaveSittings();

            OnChangedStructureEvent(item, copyPath, RegistryActionEnum.Added);
            OnChangedSizeEvent(item, copyPath, oldSize, newSize, GroupTreeSizeChangedEnum.ItemAdded);
        }

        public void Delete(IList<string> path, IGroupTreeItem item)
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
            SpaceSize oldSize = new SpaceSize(itemDirectory.Size);
            itemDirectory.Size.Subtraction(itemForDelete.Size);
            SpaceSize newSize = new SpaceSize(itemDirectory.Size);

            SaveSittings();

            OnChangedStructureEvent(itemForDelete, path, RegistryActionEnum.Removed);
            OnChangedSizeEvent(itemForDelete, path, oldSize, newSize, GroupTreeSizeChangedEnum.ItemAdded);
        }

        public void Rename(IList<string> path, string oldName, GroupTreeTypeEnum type, string newName)
        {
            string fullCurrentPath = String.Empty;
            IContainer itemDirectory = FindItemDirectory(path);

            IGroupTreeItem itemForRename = itemDirectory.Items.FirstOrDefault(it => it.Name == oldName && it.Type == type);

            if (itemForRename == null) throw new DirectoryNotFoundException();
            
            if(NameScan(itemDirectory, newName,type))
                throw new ArgumentException("Element has already been");

            itemForRename.Name = newName;

            SaveSittings();

            OnChangedGroupTreeItemNameEvent(itemForRename, path, oldName, newName);
        }

        public void Rename(IList<string> path, IGroupTreeItem item, string newName)
        {
          Rename(path,item.Name,item.Type,newName);
        }

        public void Move(IList<string> path, string name, GroupTreeTypeEnum type, IList<string> newPath)
        {
            string fullCurrentPath = String.Empty;
            IContainer itemDirectoryFrom = FindItemDirectory(path);
            IContainer itemDirectoryTo= FindItemDirectory(newPath);

            IGroupTreeItem itemForMove = itemDirectoryFrom.Items.FirstOrDefault(it => it.Name == name && it.Type == type);

            if (itemForMove == null) throw new DirectoryNotFoundException();

            if (NameScan(itemDirectoryTo, itemForMove.Name, itemForMove.Type))
                throw new ArgumentException("Element has already been");

            itemDirectoryTo.Items.Add(itemForMove);
            OnChangedStructureEvent(itemForMove, path, RegistryActionEnum.Removed);

            itemForMove.Parent = itemDirectoryTo;
            itemDirectoryFrom.Items.Remove(itemForMove);

            SaveSittings();

            OnChangedStructureEvent(itemForMove, newPath, RegistryActionEnum.Added);
        }

        public void Move(IList<string> path, IGroupTreeItem item, IList<string> newPath)
        {
            Move(path,item.Name,item.Type,newPath);
        }

        public void Copy(IList<string> path, IGroupTreeItem item, IList<string> newPath)
        {
           Copy(path,item.Name,item.Type,newPath);
        }

        public void Copy(IList<string> path, string name, GroupTreeTypeEnum type, IList<string> otherPath)
        {
            string fullCurrentPath = String.Empty;
            IContainer itemDirectoryFrom = FindItemDirectory(path);
            IContainer itemDirectoryTo = FindItemDirectory(otherPath);

            IGroupTreeItem itemForCopy = itemDirectoryFrom.Items.FirstOrDefault(it => it.Name == name && it.Type == type);

            if (itemForCopy == null) throw new DirectoryNotFoundException();

            if (NameScan(itemDirectoryTo, itemForCopy.Name, itemForCopy.Type))
                throw new ArgumentException("Element has already been");

            IGroupTreeItem newItem = itemForCopy.Clone();

            newItem.Parent = itemDirectoryTo;
            itemDirectoryTo.Items.Add(newItem);

            SaveSittings();

            OnChangedStructureEvent(newItem, otherPath, RegistryActionEnum.Added);
        }

        public void Load()
        {
            LoadSittings();
        }


        void OnChangedStructureEvent(IGroupTreeItem item, IList<string> path, RegistryActionEnum action)
        {
            ChangedStructureEvent?.Invoke(this, new GroupTreeStructureChangedEventArg(item, action){Path = path});
        }

        void OnChangedSizeEvent(IGroupTreeItem item, IList<string> path, SpaceSize oldSize, SpaceSize newSize, GroupTreeSizeChangedEnum action)
        {
            ChangedSizeEvent?.Invoke(this, new GroupTreeSizeChangedEventArg(item, oldSize, newSize, action) { Path = path });
        }

        void OnChangedGroupTreeItemNameEvent(IGroupTreeItem item, IList<string> path, string oldName, string newName)
        {
            ChangedGroupTreeItemNameEvent?.Invoke(this, new GroupTreeItemNameChangedEventArg(item, oldName, newName) { Path = path });
        }

    }
}
