using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.Settings.Accounts
{
    /// <summary>
    /// Базовый интерфейс загрузки и сохранения аккаунтов
    /// </summary>
    public interface IAccountSettings
    {
        /// <summary>
        /// Загружает колекцию аккантов
        /// </summary>
        /// <returns>Коллекция аккаунтов</returns>
        IAccountSettingsItem[] LoadAccounts();
        /// <summary>
        /// Сохраняет коллекцию аккаунтов
        /// </summary>
        /// <param name="accounts">Коллекция аккаунтов</param>
        void SaveAccounts(IAccountSettingsItem[] accounts);
    }
}
