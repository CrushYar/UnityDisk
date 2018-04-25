using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

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
        IList<IAccount> Items { get; set; }
        /// <summary>
        /// Событие после загрузки контейнера
        /// </summary>
        event EventHandler<GroupLoadedEventArg> Loaded;
        /// <summary>
        /// Событие после выгрузки контейнера
        /// </summary>
        event EventHandler<GroupUnloadedEventArg> Unload;
    }
}
