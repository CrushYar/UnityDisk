using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Accounts.Registry
{
    public sealed class RegistrySizeChangedEventArg :EventArgs
    {
        /// <summary>
        /// Старый размер
        /// </summary>
        public SpaceSize OldSize { get; set; }
        /// <summary>
        /// Новый размер
        /// </summary>
        public SpaceSize NewSize { get; set; }

        /// <summary>
        /// Аккаунт на котором произошло изменение
        /// </summary>
        public IAccount Account { get; set; }
    }
}
