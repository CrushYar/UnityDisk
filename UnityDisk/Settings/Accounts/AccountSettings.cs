using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Unity;
using UnityDisk.Accounts;

namespace UnityDisk.Settings.Accounts
{
    /// <summary>
    /// Класс отвечающий за экспорт и импрот коллекций аккантов
    /// </summary>
    public sealed class AccountSettings : IAccountSettings
    {
        private ISettings _settings;
        private string _parameterName;

        public AccountSettings(ISettings settings)
        {
            _settings = settings;
            _parameterName = "saveAcc";
        }
        public AccountSettings()
        {
            IUnityContainer container = ContainerConfiguration.GetContainer().Container;
            _settings = container.Resolve<ISettings>();
            _parameterName = "saveAcc";
        }

        public AccountSettings(ISettings settings,string parameterName)
        {
            _settings = settings;
            _parameterName = parameterName;
        }

        public IAccountSettingsItem[] LoadAccounts()
        {
           string strValue= _settings.GetValueAsString(_parameterName);

            object forGetType =
                new []{
                new AccountSettingsItem()
            };

            XmlSerializer xmlSerializer = new XmlSerializer(forGetType.GetType());
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(strValue));
            using (var reader  = new StreamReader(memoryStream, Encoding.UTF8))
            {
               return (IAccountSettingsItem[])xmlSerializer.Deserialize(reader);
            }
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
                    _settings.SetValueAsString(_parameterName, reader.ReadToEnd());
                }
            }
        }
    }
}
