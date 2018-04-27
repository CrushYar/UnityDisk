using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Accounts.Registry
{
   public interface IAccountProjection
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
        bool IsFree { get; }
        /// <summary>
        /// Коллекция названий групп в которых состоит аккаунт
        /// </summary>
        IList<String> Groups { get; }
    }
}
