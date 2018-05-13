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
using UnityDisk.Settings.Groups;
using UnityDisk.StorageItems.PreviewImageManager;

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
                .RegisterType<IAccountRegistry, UnityDisk.Accounts.Registry.AccountRegistry>(new ContainerControlledLifetimeManager(),
                    new InjectionConstructor())
                .RegisterType<IAccountSettings, UnityDisk.Settings.Accounts.AccountSettings>(new ContainerControlledLifetimeManager(),
                    new InjectionConstructor())
                .RegisterInstance<Settings.BackgroundOperations.IBackgroundOperationDispatcherSettings>(new UnityDisk.Settings.BackgroundOperations.BackgroundOperationDispatcherSettings(new Settings.LocalSettings()))
                .RegisterType<IAccount, Account>(new InjectionConstructor())
                .RegisterType<IAccountSettingsItem, AccountSettingsItem>(new InjectionConstructor())
                .RegisterType<IFactoryRagistry, FactoryRagistry>(new InjectionConstructor())
                .RegisterType<IGroupSettings, Settings.Groups.GroupSettings>(new InjectionConstructor())
                .RegisterType<IStandardPreviewImagesRegistry,StandardPreviewImagesRegistry>(new ContainerControlledLifetimeManager(),new InjectionConstructor())
                .RegisterType<IAccountProjection, AccountProjection>(new InjectionConstructor());
        }

        public static ContainerConfiguration GetContainer()
        {
            return Configuration;
        }
    }
}
