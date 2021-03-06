﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace UnityDisk.Accounts.Registry
{
   public interface IAccountRegistry
    {
        /// <summary>
        /// Имя пользователя локального компьютера
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Изображение пользователя локального компьютера
        /// </summary>
        BitmapImage UserImage { get; }

        /// <summary>
        /// Размер общего дискового пространства
        /// </summary>
        SpaceSize Size { get; }

        /// <summary>
        /// Количество зарегистрированных аккаунтов
        /// </summary>
        int Count { get; set; }

        /// <summary>
        /// Событие вызываемое после изменения размера пространства файлового хранилища аккаунта
        /// </summary>
        event EventHandler<RegistrySizeChangedEventArg> ChangedSizeEvent;

        /// <summary>
        /// Событие вызываемое после изменения реестра аккантов
        /// </summary>
        event EventHandler<RegistryChangedEventArg> ChangedRegistryEvent;

        /// <summary>
        /// Событие вызываемое после загрузки реестра
        /// </summary>
        event EventHandler<RegistryLoadedEventArg> LoadedEvent;

        /// <summary>
        /// Поиск аккаунта по логину
        /// </summary>
        /// <param name="login">Логин аккаунта</param>
        /// <returns>Найденный аккаунт</returns>
        IAccountProjection Find(String login);

        /// <summary>
        /// Регистрация аккаунта в реестре
        /// </summary>
        /// <param name="account">Аккаунт который нужно добавить в реестр</param>
        bool Registry(IAccount account);

        /// <summary>
        /// Снятие с регистрации аккаунта
        /// </summary>
        /// <param name="account">Аккаунт на удаление из реестра</param>
        bool Delete(IAccountProjection account);

        /// <summary>
        /// Проверка на наличе уже зарегистрированного аккаунта в реестре
        /// </summary>
        /// <param name="account">Аккаунт наличие которого требуется проверить</param>
        /// <returns>Результат проверки</returns>
        bool ContainsAccount(IAccountProjection account);

        /// <summary>
        /// Выполнение запросса на изменение в размере файлового пространства в указанном аккаунте
        /// </summary>
        /// <param name="login">Логин аккаунта</param>
        /// <param name="newSize">Новый размер</param>
        void ChangeAccountSize(string login, SpaceSize newSize);

        /// <summary>
        /// Выполнение запросса на изменение в размере файлового пространства в указанном аккаунте
        /// </summary>
        /// <param name="account">Аккаунт действия над которым нужно произвести</param>
        /// <param name="newSize">Новый размер</param>
        void ChangeAccountSize(IAccount account, SpaceSize newSize);

        /// <summary>
        /// Выполняет процесс загрузки данных
        /// </summary>
        void Load();
    }
}
