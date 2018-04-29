using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityDisk.GroupTree;
using UnityDisk.Settings.Accounts;

namespace UnityDisk.Settings.Groups
{
    public sealed class GroupSettings:IGroupSettings
    {
        private GroupSettingsContainer _container;
        private ISettings _settings;
        private string _parameterName;

        public GroupSettings()
        {

        }

        public GroupSettings(ISettings settings)
        {
            _settings = settings;
            _parameterName = "GroupTree";
        }
        public GroupSettings(ISettings settings,string parameterName, GroupSettingsContainer container)
        {
            _settings = settings;
            _parameterName = parameterName;
            _container = container;
        }

        public GroupSettingsContainer LoadGroupTree()
        {
            string strValue = _settings.GetValueAsString(_parameterName);
            if (String.IsNullOrEmpty(strValue))
            {
                _container=new GroupSettingsContainer(){ Items = new List<GroupSettingsItem>(), Name = "Room", IsActive = false};
                return _container;
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GroupSettingsContainer), new[]{
                typeof(GroupSettingsItem),
                typeof(GroupSettingsGroup)
            });
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(strValue));
            using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
            {
                _container=(GroupSettingsContainer)xmlSerializer.Deserialize(reader);
            }

            return _container;
        }

        public void SaveGroupTree()
        {
            Object forGetType = _container;
            XmlSerializer xmlSerializer = new XmlSerializer(forGetType.GetType(),new []{
                typeof(GroupSettingsContainer),
                typeof(GroupSettingsGroup)                
            });
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                xmlSerializer.Serialize(writer, forGetType);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    _settings.SetValueAsString(_parameterName, reader.ReadToEnd());
                }
            }
        }
        public void Add(IList<string> path, GroupSettingsItem item)
        {
            string fullCurrentPath = String.Empty;
            GroupSettingsContainer itemDirectory = FindItemDirectory(path);

            if (NameScan(itemDirectory, item.Name, item.Type))
                throw new ArgumentException("Element has already been");
         
            itemDirectory.Items.Add(item);

            SaveGroupTree();
        }

        public void Delete(IList<string> path, string name, GroupTreeTypeEnum type)
        {
            string fullCurrentPath = String.Empty;
            GroupSettingsContainer itemDirectory = FindItemDirectory(path);

            GroupSettingsItem itemForDelete = itemDirectory.Items.FirstOrDefault(it => it.Name == name && it.Type == type);

            if (itemForDelete == null) throw new DirectoryNotFoundException();

            itemDirectory.Items.Remove(itemForDelete);
          
            SaveGroupTree();
        }

        public void Rename(IList<string> path, string oldName, GroupTreeTypeEnum type, string newName)
        {
            string fullCurrentPath = String.Empty;
            GroupSettingsContainer itemDirectory = FindItemDirectory(path);

            GroupSettingsItem itemForRename = itemDirectory.Items.FirstOrDefault(it => it.Name == oldName && it.Type == type);

            if (itemForRename == null) throw new DirectoryNotFoundException();

            if (NameScan(itemDirectory, newName, type))
                throw new ArgumentException("Element has already been");

            itemForRename.Name = newName;

            SaveGroupTree();
        }

        public void Move(IList<string> path, string name, GroupTreeTypeEnum type, IList<string> newPath)
        {
            string fullCurrentPath = String.Empty;
            GroupSettingsContainer itemDirectoryFrom = FindItemDirectory(path);
            GroupSettingsContainer itemDirectoryTo = FindItemDirectory(newPath);

            GroupSettingsItem itemForMove = itemDirectoryFrom.Items.FirstOrDefault(it => it.Name == name && it.Type == type);

            if (itemForMove == null) throw new DirectoryNotFoundException();

            if (NameScan(itemDirectoryTo, itemForMove.Name, itemForMove.Type))
                throw new ArgumentException("Element has already been");

            itemDirectoryTo.Items.Add(itemForMove);

            itemDirectoryFrom.Items.Remove(itemForMove);

            SaveGroupTree();
        }

        public void Copy(IList<string> path, string name, GroupTreeTypeEnum type, IList<string> otherPath)
        {
            string fullCurrentPath = String.Empty;
            GroupSettingsContainer itemDirectoryFrom = FindItemDirectory(path);
            GroupSettingsContainer itemDirectoryTo = FindItemDirectory(otherPath);

            GroupSettingsItem itemForCopy = itemDirectoryFrom.Items.FirstOrDefault(it => it.Name == name && it.Type == type);

            if (itemForCopy == null) throw new DirectoryNotFoundException();

            if (NameScan(itemDirectoryTo, itemForCopy.Name, itemForCopy.Type))
                throw new ArgumentException("Element has already been");

            itemDirectoryTo.Items.Add(itemForCopy);

            SaveGroupTree();
        }

        public void SetActive(IList<string> path, string name, bool value)
        {
            string fullCurrentPath = String.Empty;
            GroupSettingsContainer itemDirectory = FindItemDirectory(path);

            GroupSettingsContainer itemForSetActive = itemDirectory.Items.FirstOrDefault(it => it.Name == name && it.Type == GroupTreeTypeEnum.Container) as GroupSettingsContainer;

            if (itemForSetActive == null) throw new DirectoryNotFoundException();

            bool oldValue = itemForSetActive.IsActive;
            itemForSetActive.IsActive = value;

            SaveGroupTree();
        }

        /// <summary>
        /// Сканирует контейнер на наличие элемента
        /// </summary>
        /// <param name="container">Контейнер для анализа</param>
        /// <param name="name">Имя элемента</param>
        /// <param name="type">Тип элемента</param>
        /// <returns>Возвращает true - при наличии элемента с указанным именем и типом</returns>
        private bool NameScan(GroupSettingsContainer container, string name, GroupTreeTypeEnum type)
        {
            int hashName = name.GetHashCode();
            foreach (var item in container.Items)
            {
                if (item.Type != type) continue;
                if (item.Name.GetHashCode() == hashName) return true;
            }

            return false;
        }
        private GroupSettingsContainer FindItemDirectory(IList<string> path)
        {
            string fullCurrentPath = String.Empty;
            GroupSettingsContainer walkerToBottom = _container;

            foreach (var containernNme in path)
            {
                fullCurrentPath += "/" + containernNme;
                walkerToBottom =
                    walkerToBottom.Items.First(it =>
                        it.Name == containernNme && it.Type == GroupTreeTypeEnum.Container) as GroupSettingsContainer;
                if (walkerToBottom == null)
                    throw new InvalidDataException(String.Concat("Don't found the container: '{0}'",
                        fullCurrentPath));
            }

            return walkerToBottom;
        }
    }
}
