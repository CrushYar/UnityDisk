﻿using System;
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
using UnityDisk.Accounts.Registry.Request;

namespace UnityDisk.Accounts.Registry
{
    public class AccountRegistry : IAccountRegistry
    {
        /// <summary>
        /// реестр аккаунтов, где логин является ключем
        /// </summary>
        private Dictionary<string, IAccount> _accounts;
        /// <summary>
        /// Настройки аккаунта
        /// </summary>
        private IAccountSettings _settings;
        /// <summary>
        /// loc контейнер с настройками
        /// </summary>
        private IContainer _settingsContainer;
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
            _settingsContainer = ContainerConfigurationForSettings.GetContainer().Container;
            _settings = _settingsContainer.GetInstance<IAccountSettings>();
            _accounts =new Dictionary<string, IAccount>(10);
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
                OnChangedSizeEvent(oldSize,account);
                OnChangedRegistryEvent(account,RegistryActionEnum.AddedAccount);
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
                OnChangedSizeEvent(oldSize, account);
                OnChangedRegistryEvent(account, RegistryActionEnum.RemovedAccount);
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

        public void AccountChangeRequest(string login, IChangeAccountRequest request)
        {
            throw new NotImplementedException();
        }

        public void AccountChangeRequest(IAccount account, IChangeAccountRequest request)
        {
            throw new NotImplementedException();
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
    }
}
