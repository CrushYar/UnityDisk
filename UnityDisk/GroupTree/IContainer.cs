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
        IList<IGroupTreeItem> Items { get; }
        /// <summary>
        /// Событие после загрузки контейнера
        /// </summary>
        event EventHandler<ContainerLoadedEventArg> LoadedEvent;
        /// <summary>
        /// Событие после выгрузки контейнера
        /// </summary>
        event EventHandler<ContainerUnloadedEventArg> UnloadEvent;
    }
}
