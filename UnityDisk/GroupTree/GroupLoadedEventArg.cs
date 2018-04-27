using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.GroupTree
{
    /// <summary>
    /// Параметр возвращаемый событием после загрузки группы
    /// </summary>
    public sealed  class GroupLoadedEventArg :EventArgs
    {
        public GroupLoadedEventArg(IList<IAccount> items)
        {
            Items = items;
        }
        /// <summary>
        /// Общий размер группы
        /// </summary>
        SpaceSize Size { get; set; }
        /// <summary>
        /// Колекция аккаунтов входящая в состав группы
        /// </summary>
        public IList<IAccount> Items { get; set; }
    }
}
