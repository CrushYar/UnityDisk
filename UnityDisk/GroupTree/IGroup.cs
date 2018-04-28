using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;

namespace UnityDisk.GroupTree
{ 
    /// <summary>
    /// Интерфейс группы
    /// </summary>
    public interface IGroup:IGroupTreeItem
    {
        /// <summary>
        /// Коллекция аккаунтов находящиеся в группе
        /// </summary>
        IList<IAccountProjection> Items { get; set; }
    }
}
