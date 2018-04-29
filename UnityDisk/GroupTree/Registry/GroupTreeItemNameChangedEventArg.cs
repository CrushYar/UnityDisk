using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.GroupTree.Registry
{
   public class GroupTreeItemNameChangedEventArg:EventArgs
    {
        public GroupTreeItemNameChangedEventArg(IGroupTreeItem item, string oldName, string newName)
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
            OldName = oldName;
            NewName = newName;
        }

        public string OldName { get; set; }
        public string NewName { get; set; }
        public IGroupTreeItemProjection Item { get; set; }
        /// <summary>
        /// Путь к элементу
        /// </summary>
        public Queue<string> Path { get; set; }
    }
}
