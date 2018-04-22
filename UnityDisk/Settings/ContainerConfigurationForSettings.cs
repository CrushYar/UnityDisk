using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using UnityDisk.Settings.Accounts;

namespace UnityDisk.Settings
{
    public class ContainerConfigurationForSettings
    {
        private static ContainerConfigurationForSettings _settings=new ContainerConfigurationForSettings();

        public IContainer Container { get; private set; }

        private ContainerConfigurationForSettings()
        {
            Container = new Container(x => x.For<ISettings>().Singleton().Use<RemoteSettings>());
            Container.Configure(x => x.For<IAccountSettings>().Singleton().Use<AccountSettings>());
            Container.Configure(x => x.For<IAccountSettingsItem>().Use<AccountSettingsItem>());
        }

        public static ContainerConfigurationForSettings GetContainer()
        {
            return _settings;
        }
    }
}
