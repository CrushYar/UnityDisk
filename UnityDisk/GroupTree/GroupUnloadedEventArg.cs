using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.GroupTree
{
    /// <summary>
    /// Параметр возвращаемый событием после выгрузки группы
    /// </summary>
    public class GroupUnloadedEventArg:EventArgs
    {
        public GroupUnloadedEventArg(IList<IAccount> items)
        {
            Items = items;
        }
        /// <summary>
        /// Колекция аккаунтов входящая в состав группы
        /// </summary>
        public IList<IAccount> Items { get; set; }
    }
}
