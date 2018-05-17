using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Unity;
using UnityDisk.GroupTree;
using UnityDisk.Settings;
using UnityDisk.Settings.Accounts;

namespace UnityDisk.Accounts.Registry
{
    public sealed class AccountRegistry : IAccountRegistry
    {
        private SpaceSize _size=new SpaceSize();
        private BitmapImage _userImage;
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
        private readonly IUnityContainer _container;
        /// <summary>
        /// Объект синхронизации
        /// </summary>
        private SpinLock _spinLock=new SpinLock(true);
        /// <summary>
        /// Имя пользователя локального компьютера
        /// </summary>
        public string UserName { get;private set; }
        /// <summary>
        /// Изображение пользователя локального компьютера
        /// </summary>
        public BitmapImage UserImage { get=>new BitmapImage(_userImage.UriSource); private set=> _userImage=value ; }
        /// <summary>
        /// Общее пространство
        /// </summary>
        public SpaceSize Size {get => new SpaceSize(_size); private set=>_size=value; }
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
            _settings = _container.Resolve<IAccountSettings>();
            _accounts =new Dictionary<string, IAccount>(10);
        }

        /// <summary>
        /// Используется для Unit Test
        /// </summary>
        /// <param name="settingsContainer"></param>
        /// <param name="accountContainer"></param>
        public AccountRegistry(IAccountSettings settings, IUnityContainer accountContainer)
        {
            _container = accountContainer;
            _settings = settings;
            _accounts = new Dictionary<string, IAccount>(10);
        }
        public IAccountProjection Find(string login)
        {
            Lock();
            _accounts.TryGetValue(login, out IAccount value);
            UnLock();
            if (value == null) return null;
            IAccountProjection projection= _container.Resolve<IAccountProjection>();
            projection.SetDataContext(value);
            return projection;
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
                SpaceSize oldSize = _size;
                _size.TotalSize += account.Size.TotalSize;
                _size.UsedSize += account.Size.UsedSize;
                _size.FreeSize += account.Size.FreeSize;
                SaveSettings();
                OnChangedSizeEvent(oldSize,account);
                OnChangedRegistryEvent(account, RegistryActionEnum.Added);
            }

            return !isContains;
        }
        public bool Delete(IAccountProjection account)
        {
            return Delete(account.Login);
        }
        public bool Delete(string login)
        {
            Lock();
            _accounts.TryGetValue(login,out IAccount  accountForRemoved);
            bool isContains = accountForRemoved != null;
            if (isContains)
            {
                _accounts.Remove(login);
            }
            UnLock();
            if (isContains)
            {
                SpaceSize oldSize = Size;
                _size.TotalSize -= accountForRemoved.Size.TotalSize;
                _size.UsedSize -= accountForRemoved.Size.UsedSize;
                _size.FreeSize -= accountForRemoved.Size.FreeSize;
                SaveSettings();
                OnChangedSizeEvent(oldSize, accountForRemoved);
                OnChangedRegistryEvent(accountForRemoved, RegistryActionEnum.Removed);
            }
            return isContains;
        }
        public bool ContainsAccount(IAccountProjection account)
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

            _size.TotalSize = 0;
            _size.UsedSize = 0;
            _size.FreeSize = 0;

            Lock();
            
            foreach (var settingsItem in accountSettingsItems)
            {
                var account = _container.Resolve<IAccount>();
                account.Login = settingsItem.Login;
                account.Token = settingsItem.Token;

                if (account.LoadConnector(settingsItem.ServerName))
                {
                    _accounts.Add(settingsItem.Login, account);
                    _size.TotalSize += account.Size.TotalSize;
                    _size.UsedSize += account.Size.UsedSize;
                    _size.FreeSize += account.Size.FreeSize;
                }
            }
            UnLock();
        }

        /// <summary>
        /// Сохранение аккаунтов
        /// </summary>
        private void SaveSettings()
        {
            Lock();
            IAccountSettingsItem[] settingsItems= new AccountSettingsItem[_accounts.Count];
            int i = 0;
            foreach (var account in _accounts)
            {
                settingsItems[i++]=new AccountSettingsItem(account.Value);
            }
            UnLock();
            _settings.SaveAccounts(settingsItems);
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
            IAccountProjection projection=null;
            if (account != null)
            {
                projection = _container.Resolve<IAccountProjection>();
                projection.SetDataContext(account);
            }

            ChangedRegistryEvent?.Invoke(this,
                new RegistryChangedEventArg() { Account = projection, Action = action });
        }
        private void OnChangedSizeEvent(SpaceSize oldSize, IAccount account)
        {
            IAccountProjection projection = null;
            if (account != null)
            {
                projection = _container.Resolve<IAccountProjection>();
                projection.SetDataContext(account);
            }
            ChangedSizeEvent?.Invoke(this,
                new RegistrySizeChangedEventArg() { Account = projection, OldSize = oldSize, NewSize = this.Size });
        }
        private void OnLoadedEvent()
        {
            IAccountProjection[] loadedAccounts=new IAccountProjection[_accounts.Count];
            Lock();
            int i = 0;
            foreach (var account in _accounts)
            {
                IAccountProjection projection = _container.Resolve<IAccountProjection>();
                projection.SetDataContext(account.Value);
                loadedAccounts[i]= projection;
                i++;
            }
            UnLock();
            LoadedEvent?.Invoke(this,
                new RegistryLoadedEventArg() {Accounts = loadedAccounts , Size =Size});
        }

        public void ChangeAccountSize(string login, SpaceSize newSize)
        {
            Lock();
            _accounts.TryGetValue(login,out IAccount account);
            UnLock();
            if(account==null)return;
            SpaceSize oldSize = Size;
            account.Size=new SpaceSize(newSize);
            //SaveSettings();
            OnChangedSizeEvent(oldSize,account);
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
