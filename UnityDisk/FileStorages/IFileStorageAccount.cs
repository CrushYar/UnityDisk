using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.FileStorages
{
    public interface IFileStorageAccount
    {
        /// <summary>
        /// Логин
        /// </summary>
        string Login { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        DateTime CreateDate { get; set; }
        /// <summary>
        /// Информация о состоянии памяти файлового хранилища аккаунта
        /// </summary>
        SpaceSize Size { get; set; }
        /// <summary>
        /// Имя сервера
        /// </summary>
        string ServerName { get; set; }
        /// <summary>
        /// Токен для работы с файловым хранилищем аккаунта
        /// </summary>
        string Token { get; set; }
        /// <summary>
        /// Указывает на отсутствие привязки к группе
        /// </summary>
        bool IsFree { get; set; }
        /// <summary>
        /// Статус подключения
        /// </summary>
        ConnectionStatusEnum Status { get; set; }

        /// <summary>
        /// Вход в аккаунт
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task SignIn(String key);
        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        /// <returns></returns>
        Task SignOut();
        /// <summary>
        /// Обновление информации об аккауте
        /// </summary>
        /// <returns></returns>
        Task Update();
    }
}
