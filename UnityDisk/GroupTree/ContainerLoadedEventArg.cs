using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.GroupTree
{
    /// <summary>
    /// Параметр возвращаемый событием после загрузки контейнера
    /// </summary>
    public class ContainerLoadedEventArg : EventArgs
    {
        public ContainerLoadedEventArg(IList<IGroupTreeItem> items)
        {
            Items = items;
        }
        /// <summary>
        /// Общий размер контейнера
        /// </summary>
        SpaceSize Size { get; set; }
        /// <summary>
        /// Колекция загруженных дочерних элементов группы
        /// </summary>
        public IList<IGroupTreeItem> Items { get; set; }
    }
}
