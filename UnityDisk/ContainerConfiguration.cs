using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using UnityDisk.Accounts;
using UnityDisk.FileStorages.FactoryRagistry;
using UnityDisk.Settings;
using UnityDisk.Settings.Accounts;

namespace UnityDisk
{
    public class ContainerConfiguration
    {
        private static readonly ContainerConfiguration Configuration=new ContainerConfiguration();

        public IContainer Container { get; private set; }

        private ContainerConfiguration()
        {
            Container = new Container(x => x.For<ISettings>().Singleton().Use<RemoteSettings>());
            Container.Configure(x => x.For<Accounts.IAccount>().Use<Accounts.Account>().SelectConstructor(()=>new Account()));
            Container.Configure(x => x.For<IAccountSettings>().Singleton().Use<AccountSettings>().SelectConstructor(()=>new AccountSettings()));
            Container.Configure(x => x.For<IAccountSettingsItem>().Use<AccountSettingsItem>().SelectConstructor(()=>new AccountSettingsItem()));
            Container.Configure(x => x.For<IFactoryRagistry>().Use<FactoryRagistry>().SelectConstructor(()=>new FactoryRagistry()));
        }

        public static ContainerConfiguration GetContainer()
        {
            return Configuration;
        }
    }
}
