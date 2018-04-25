using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.GroupTree
{
    /// <summary>
    /// Параметр возвращаемый событием после выгрузки контейнера
    /// </summary>
    public class ContainerUnloadedEventArg : EventArgs
    {
        public ContainerUnloadedEventArg(IList<IGroupTreeItem> items)
        {
            Items = items;
        }
        public IList<IGroupTreeItem> Items { get; set; }
    }
}
