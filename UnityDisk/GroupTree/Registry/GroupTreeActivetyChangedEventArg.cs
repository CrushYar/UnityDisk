using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.GroupTree.Registry
{
   public class GroupTreeActivetyChangedEventArg:EventArgs
    {
        public GroupTreeActivetyChangedEventArg(IContainer container, bool oldValue, bool newValue)
        {
            Item = new ContainerProjection(container);
            OldValue = oldValue;
            NewValue = newValue;
        }
        /// <summary>
        /// Старое имя
        /// </summary>
        public bool OldValue{ get; set; }
        /// <summary>
        /// Новое имя
        /// </summary>
        public bool NewValue { get; set; }
        public IGroupTreeItemProjection Item { get; set; }
        /// <summary>
        /// Путь к элементу
        /// </summary>
        public IList<string> Path { get; set; }
    }
}
