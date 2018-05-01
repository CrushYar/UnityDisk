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
        ItemAdded, ItemDeleted, ItemChanged,ItemCopied,ItemMoved
    }
    /// <summary>
    /// Параметр события об изменении размера
    /// </summary>
    public sealed class GroupTreeSizeChangedEventArg:EventArgs
    {
        public GroupTreeSizeChangedEventArg(SpaceSize oldSize, SpaceSize newSize, GroupTreeSizeChangedEnum action)
        {
            OldSize = oldSize;
            NewSize = newSize;
            Action = action;
        }

        /// <summary>
        /// Тип события
        /// </summary>
        public GroupTreeSizeChangedEnum Action { get; private set; }
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
