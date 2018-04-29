using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityDisk.Settings.Accounts;

namespace UnityDisk.Settings.Groups
{
    public sealed class GroupSettings:IGroupSettings
    {
        private GroupSettingsContainer _container;
        private ISettings _settings;
        private string _parameterName;

        public GroupSettings() { }

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
    }
}
