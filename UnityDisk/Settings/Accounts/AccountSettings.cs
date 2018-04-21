using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using StructureMap;
using UnityDisk.Accounts;

namespace UnityDisk.Settings.Accounts
{
    /// <summary>
    /// Класс отвечающий за экспорт и импрот коллекций аккантов
    /// </summary>
    public class AccountSettings : IAccountSettings
    {
        private ISettings _settings;

        public AccountSettings()
        {
            var settingsContainer = ContainerConfigurationForSettings.GetContainer().Container;
            _settings = settingsContainer.GetInstance<ISettings>();
        }

        public AccountSettings(ISettings settings)
        {
            _settings = settings;
        }

        public IAccountSettingsItem[] LoadAccounts()
        {
            throw new NotImplementedException();
        }

        public void SaveAccounts(IAccountSettingsItem[] accounts)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(accounts.GetType());
            var memoryStream=new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                xmlSerializer.Serialize(writer, accounts);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    _settings.SetValueAsString("saveAcc",reader.ReadToEnd());
                }
            }
        }
    }
}
