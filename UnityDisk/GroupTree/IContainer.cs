using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.GroupTree
{
    /// <summary>
    /// Интерфейс контейнера групп
    /// </summary>
    public interface IContainer :IGroupTreeItem
    {
        /// <summary>
        /// Коллекция дочерних элементов групп
        /// </summary>
        IList<IGroupTreeItem> Items { get; set; }
        /// <summary>
        /// Режим контейнера
        /// </summary>
        bool IsActive { get; set; }
    }
}
