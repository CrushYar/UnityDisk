using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Accounts.Registry
{
    public enum RegistryActionEnum
    {
        AddedAccount, RemovedAccount
    }
    public class RegistryChangedEventArg:EventArgs
    {
        /// <summary>
        /// Аккаунт на котором произошло изменение
        /// </summary>
        public IAccount Account { get; set; }
        /// <summary>
        /// Тип действия
        /// </summary>
        public RegistryActionEnum Action { get; set; }
    }
}
