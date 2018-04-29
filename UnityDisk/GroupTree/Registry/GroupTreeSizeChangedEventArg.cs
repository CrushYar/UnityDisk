using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.GroupTree.Registry
{
    /// <summary>
    /// Тип события, который произвел изменения в размере
    /// </summary>
    public enum GroupTreeSizeChangedEnum
    {
        ItemAdded, ItemDeleted, ItemChanged
    }
    /// <summary>
    /// Параметр события об изменении размера
    /// </summary>
    public class GroupTreeSizeChangedEventArg:EventArgs
    {
        public GroupTreeSizeChangedEventArg(IGroupTreeItem item, SpaceSize oldSize, SpaceSize newSize, GroupTreeSizeChangedEnum action)
        {
            switch (item)
            {
                case IGroup group:
                    Item = new GroupProjection(group);
                    break;
                case IContainer container:
                    Item = new ContainerProjection(container);
                    break;
                default:
                    throw new ArgumentException("Unknown type");
            }
            OldSize = oldSize;
            NewSize = newSize;
            Action = action;
        }

        /// <summary>
        /// Тип события
        /// </summary>
        public GroupTreeSizeChangedEnum Action { get; private set; }
        /// <summary>
        /// Элемент, который вызвал событие
        /// </summary>
        public IGroupTreeItemProjection Item { get; private set; }
        /// <summary>
        /// Путь к элементу
        /// </summary>
        public Queue<string> Path { get; set; }
        /// <summary>
        /// Старый размер
        /// </summary>
        public SpaceSize OldSize { get; private set; }
        /// <summary>
        /// Новый размер
        /// </summary>
        public SpaceSize NewSize { get; private set; }
    }
}
