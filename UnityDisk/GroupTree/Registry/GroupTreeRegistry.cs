using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition.Interactions;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;

namespace UnityDisk.GroupTree.Registry
{
    /// <summary>
    /// Реестр групп
    /// </summary>
    public class GroupTreeRegistry:IGroupTreeRegistry
    {
        private IContainer _groupTree;

        public event EventHandler<GroupTreeSizeChangedEventArg> ChangedSizeEvent;
        public event EventHandler<GroupTreeStructureChangedEventArg> ChangedStructureEvent;

        private void LoadSittings() { }
        private void SaveSittings() { }

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

        public void Add(Queue<string> path, IGroupTreeItem item)
        {
            string fullCurrentPath = String.Empty;
            IContainer walkerToBottom= _groupTree;


            string name = path.Count > 0 ? path.Dequeue() : null ;
            while (name != null)
            {
                fullCurrentPath += "/" + name;
                walkerToBottom = walkerToBottom.Items.First(it => it.Name == name && it.Type == GroupTreeTypeEnum.Container) as IContainer;
                if (walkerToBottom == null)
                    throw new InvalidDataException(String.Concat("Don't found the container: '{0}'", fullCurrentPath));
                name = path.Count > 0 ? path.Dequeue() : null;
            }

            if (NameScan(walkerToBottom, item.Name,item.Type))
                throw new ArgumentException("Element has already been");

            item.Parent = walkerToBottom;
            walkerToBottom.Items.Add(item);

            IContainer walkerToTop= walkerToBottom;
            SpaceSize oldSize = new SpaceSize(walkerToTop.Size);
            walkerToTop.Size.Addition(item.Size);
            SpaceSize newSize = new SpaceSize(walkerToTop.Size);
            walkerToTop = walkerToTop.Parent;

            while (walkerToTop != null)
            {
                walkerToTop.Size.Addition(item.Size);
                walkerToTop = walkerToTop.Parent;
            }

            OnChangedStructureEvent(item, RegistryActionEnum.Added);
            OnChangedSizeEvent(item, oldSize, newSize, GroupTreeSizeChangedEnum.ItemAdded);
        }

        public void Delete(Queue<string> fullPath, GroupTreeTypeEnum type)
        {
            string fullCurrentPath = String.Empty;
            IContainer walkerToBottom = _groupTree;


            while (fullPath.Count > 1)
            {
                string containernNme = fullPath.Dequeue();
                fullCurrentPath += "/" + containernNme;
                walkerToBottom = walkerToBottom.Items.First(it => it.Name == containernNme && it.Type==GroupTreeTypeEnum.Container) as IContainer;
                if (walkerToBottom == null)
                    throw new InvalidDataException(String.Concat("Don't found the container: '{0}'", fullCurrentPath));
            }

            string itemName = fullPath.Count > 0 
                ? fullPath.Dequeue() : throw new ArgumentException("There isn't the name of item");

            IGroupTreeItem itemForDelete= walkerToBottom.Items.FirstOrDefault(it => it.Name == itemName && it.Type == type);

            if(itemForDelete==null) throw new DirectoryNotFoundException();

            walkerToBottom.Items.Remove(itemForDelete);
            SpaceSize oldSize = new SpaceSize(walkerToBottom.Size);
            walkerToBottom.Size.Subtraction(itemForDelete.Size);
            SpaceSize newSize = new SpaceSize(walkerToBottom.Size);

            OnChangedStructureEvent(itemForDelete, RegistryActionEnum.Removed);
            OnChangedSizeEvent(itemForDelete, oldSize, newSize, GroupTreeSizeChangedEnum.ItemAdded);
        }

        public void Delete(Queue<string> path, IGroupTreeItem item)
        {
            string fullCurrentPath = String.Empty;
            IContainer walkerToBottom = _groupTree;


            while (path.Count > 0)
            {
                string containernNme = path.Dequeue();
                fullCurrentPath += "/" + containernNme;
                walkerToBottom = walkerToBottom.Items.First(it => it.Name == containernNme && it.Type == GroupTreeTypeEnum.Container) as IContainer;
                if (walkerToBottom == null)
                    throw new InvalidDataException(String.Concat("Don't found the container: '{0}'", fullCurrentPath));
            }

            IGroupTreeItem itemForDelete = walkerToBottom.Items.FirstOrDefault(it => it.Name == item.Name && it.Type == item.Type);

            if (itemForDelete == null) throw new DirectoryNotFoundException();

            walkerToBottom.Items.Remove(itemForDelete);
            SpaceSize oldSize = new SpaceSize(walkerToBottom.Size);
            walkerToBottom.Size.Subtraction(itemForDelete.Size);
            SpaceSize newSize = new SpaceSize(walkerToBottom.Size);

            OnChangedStructureEvent(itemForDelete, RegistryActionEnum.Removed);
            OnChangedSizeEvent(itemForDelete, oldSize, newSize, GroupTreeSizeChangedEnum.ItemAdded);
        }

        public void Rename(Queue<string> path, string oldName, GroupTreeTypeEnum type, string newName)
        {
            throw new NotImplementedException();
        }

        public void Rename(Queue<string> path, IGroupTreeItem item, string newName)
        {
            throw new NotImplementedException();
        }

        public void Move(Queue<string> oldFullPath, GroupTreeTypeEnum type, Queue<string> newPath)
        {
            throw new NotImplementedException();
        }

        public void Move(Queue<string> path, IGroupTreeItem item, Queue<string> newPath)
        {
            throw new NotImplementedException();
        }

        public void Copy(Queue<string> path, IGroupTreeItem item, Queue<string> newPath)
        {
            throw new NotImplementedException();
        }

        public void Copy(Queue<string> fullPath, GroupTreeTypeEnum type, Queue<string> otherPath)
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }


        void OnChangedStructureEvent(IGroupTreeItem item, RegistryActionEnum action)
        {
            ChangedStructureEvent?.Invoke(this, new GroupTreeStructureChangedEventArg(item, action));
        }

        void OnChangedSizeEvent(IGroupTreeItem item, SpaceSize oldSize, SpaceSize newSize, GroupTreeSizeChangedEnum action)
        {
            ChangedSizeEvent?.Invoke(this, new GroupTreeSizeChangedEventArg(item, oldSize, newSize, action));
        }
    }
}
