using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;
using UnityDisk.FileStorages.FactoryRagistry;
using UnityDisk.Settings;
using UnityDisk.Settings.Accounts;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using UnityDisk.Accounts.Registry;

namespace UnityDisk
{
    public sealed class ContainerConfiguration
    {
        private static readonly ContainerConfiguration Configuration=new ContainerConfiguration();

        public IUnityContainer Container { get; private set; }

        private ContainerConfiguration()
        {
            Container= new UnityContainer();
            Container.RegisterType<ISettings, RemoteSettings>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor())
                .RegisterType<IAccount, Account>(new InjectionConstructor())
                .RegisterType<IAccountSettingsItem, AccountSettingsItem>(new InjectionConstructor())
                .RegisterType<IFactoryRagistry, FactoryRagistry>(new InjectionConstructor())
                .RegisterType<IAccountProjection, AccountProjection>(new InjectionConstructor());
        }

        public static ContainerConfiguration GetContainer()
        {
            return Configuration;
        }
    }
}
