using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using StructureMap;
using UnityDisk.Settings;
using UnityDisk.Settings.Accounts;

namespace UnityDisk.Accounts.Registry
{
    public class AccountRegistry : IAccountRegistry
    {
        /// <summary>
        /// реестр аккаунтов, где логин является ключем
        /// </summary>
        private readonly Dictionary<string, IAccount> _accounts;
        /// <summary>
        /// Настройки аккаунта
        /// </summary>
        private readonly IAccountSettings _settings;
        /// <summary>
        /// loc контейнер с настройками
        /// </summary>
        private readonly IContainer _container;
        /// <summary>
        /// Объект синхронизации
        /// </summary>
        private SpinLock _spinLock=new SpinLock(true);
        /// <summary>
        /// Имя пользователя локального компьютера
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Изображение пользователя локального компьютера
        /// </summary>
        public BitmapImage UserImage { get; set; }
        /// <summary>
        /// Общее пространство
        /// </summary>
        public SpaceSize Size { get; set; }
        /// <summary>
        /// Количество зарегистрированных аккаунтов
        /// </summary>
        public int Count { get; set; }

        public event EventHandler<RegistrySizeChangedEventArg> ChangedSizeEvent;
        public event EventHandler<RegistryChangedEventArg> ChangedRegistryEvent;
        public event EventHandler<RegistryLoadedEventArg> LoadedEvent;

        public AccountRegistry()
        {
            _container = ContainerConfiguration.GetContainer().Container;
            _settings = _container.GetInstance<IAccountSettings>();
            _accounts =new Dictionary<string, IAccount>(10);
        }

        /// <summary>
        /// Используется для Unit Test
        /// </summary>
        /// <param name="settingsContainer"></param>
        /// <param name="accountContainer"></param>
        public AccountRegistry(IContainer settingsContainer, IContainer accountContainer)
        {
            _container = accountContainer;
            _settings = settingsContainer.GetInstance<IAccountSettings>();
            _accounts = new Dictionary<string, IAccount>(10);
        }
        public IAccount Find(string login)
        {
            Lock();
            _accounts.TryGetValue(login, out IAccount value);
            UnLock();
            return value;
        }

        public bool Registry(IAccount account)
        {
            bool isContains;
            Lock();
            isContains = _accounts.ContainsKey(account.Login);
            if(!isContains){
                _accounts.Add(account.Login, account);
            }
            UnLock();
            if (!isContains)
            {
                SpaceSize oldSize =new SpaceSize(Size);
                Size.TotalSize += account.Size.TotalSize;
                Size.UsedSize += account.Size.UsedSize;
                Size.FreelSize += account.Size.FreelSize;
                SaveSettings();
                OnChangedSizeEvent(oldSize,account.Clone());
                OnChangedRegistryEvent(account.Clone(), RegistryActionEnum.AddedAccount);
            }

            return !isContains;
        }
        public bool Delete(IAccount account)
        {
            bool isContains;
            Lock();
            isContains = _accounts.ContainsKey(account.Login);
            if (isContains)
            {
                _accounts.Remove(account.Login);
            }
            UnLock();
            if (isContains)
            {
                SpaceSize oldSize = new SpaceSize(Size);
                Size.TotalSize += account.Size.TotalSize;
                Size.UsedSize += account.Size.UsedSize;
                Size.FreelSize += account.Size.FreelSize;
                SaveSettings();
                OnChangedSizeEvent(oldSize, account.Clone());
                OnChangedRegistryEvent(account.Clone(), RegistryActionEnum.RemovedAccount);
            }
            return isContains;
        }
        public bool ContainsAccount(IAccount account)
        {
            Lock();
            bool result= _accounts.ContainsKey(account.Login);
            UnLock();
            return result;
        }

        /// <summary>
        /// Загрузка аккаунтов
        /// </summary>
        private void LoadSettings()
        {
            IAccountSettingsItem[] accountSettingsItems = _settings.LoadAccounts();

            Lock();

            foreach (var settingsItem in accountSettingsItems)
            {
                var account = _container.GetInstance<IAccount>();
                account.Login = settingsItem.Login;
                account.Token = settingsItem.Token;
                account.ServerName = settingsItem.ServerName;

                if (account.LoadServer(settingsItem.ServerName))
                    _accounts.Add(settingsItem.Login, account);
            }
            UnLock();
        }

        /// <summary>
        /// Сохранение аккаунтов
        /// </summary>
        private void SaveSettings()
        {

        }

        private void Lock()
        {
            bool taken = false;
            _spinLock.Enter(ref taken);
        }
        private void UnLock()
        {
            _spinLock.Exit();
        }

        private void OnChangedRegistryEvent(IAccount account, RegistryActionEnum action)
        {
            ChangedRegistryEvent?.Invoke(this,
                new RegistryChangedEventArg() { Account = account, Action = action });
        }
        private void OnChangedSizeEvent(SpaceSize oldSize, IAccount account)
        {
            ChangedSizeEvent?.Invoke(this,
                new RegistrySizeChangedEventArg() { Account = account, OldSize = oldSize, NewSize = this.Size });
        }
        private void OnLoadedEvent()
        {
            IAccount[] loadedAccounts=new IAccount[_accounts.Count];
            Lock();
            int i = 0;
            foreach (var account in _accounts)
            {
                loadedAccounts[i]=account.Value.Clone();
                i++;
            }
            UnLock();
            LoadedEvent?.Invoke(this,
                new RegistryLoadedEventArg() {Accounts = loadedAccounts });
        }

        public void ChangeAccountSize(string login, SpaceSize newSize)
        {
            Lock();
            _accounts.TryGetValue(login,out IAccount account);
            UnLock();
            if(account==null)return;
            SpaceSize oldSize = Size;
            account.Size=new SpaceSize(newSize);
            SaveSettings();
            OnChangedSizeEvent(oldSize,account.Clone());
        }

        public void ChangeAccountSize(IAccount account, SpaceSize newSize)
        {
            ChangeAccountSize(account.Login, newSize);
        }

        public void Load()
        {
            Lock();
            _accounts.Clear();
            UnLock();

            OnChangedRegistryEvent(null,RegistryActionEnum.Reseted);

            LoadSettings();
            OnLoadedEvent();
        }
    }
}
