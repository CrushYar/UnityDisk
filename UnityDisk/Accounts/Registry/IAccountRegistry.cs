using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace UnityDisk.Accounts.Registry
{
    interface IAccountRegistry
    {
        /// <summary>
        /// Имя пользователя локального компьютера
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// Изображение пользователя локального компьютера
        /// </summary>
        BitmapImage UserImage { get; set; }
        /// <summary>
        /// Размер общего дискового пространства
        /// </summary>
        SpaceSize Size { get; set; }
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
        IAccount Find(String login);
        /// <summary>
        /// Регистрация аккаунта в реестре
        /// </summary>
        /// <param name="account"></param>
        void Registry(IAccount account);
    }
}
