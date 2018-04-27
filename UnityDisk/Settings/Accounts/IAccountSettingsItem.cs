using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Settings.Accounts
{
    public interface IAccountSettingsItem: IEquatable<IAccountSettingsItem>,IComparable<IAccountSettingsItem>
    {
        /// <summary>
        /// Логин
        /// </summary>
        string Login { get; set; }
        /// <summary>
        /// Токен
        /// </summary>
        string Token { get; set; }
        /// <summary>
        /// Название сервера
        /// </summary>
        string ServerName { get; set; }
    }
}
