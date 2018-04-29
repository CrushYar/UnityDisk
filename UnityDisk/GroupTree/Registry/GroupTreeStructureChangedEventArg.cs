using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts.Registry;

namespace UnityDisk.GroupTree.Registry
{
    /// <summary>
    /// Параметр события об изменении структуры дерева групп
    /// </summary>
    public class GroupTreeStructureChangedEventArg:EventArgs
    {
        public GroupTreeStructureChangedEventArg(IGroupTreeItem item, RegistryActionEnum action)
        {
            Action = action;
            switch (item)
            {
                case IGroup group:
                    Item=new GroupProjection(group);
                    break;
                case IContainer container:
                    Item = new ContainerProjection(container);
                    break;
                default:
                    throw new ArgumentException("Unknown type");
            }
        }

        /// <summary>
        /// Элемент выполнивший действие
        /// </summary>
        public IGroupTreeItemProjection Item { get; private set; }
        /// <summary>
        /// Путь к элементу
        /// </summary>
        public Queue<string> Path { get; set; }
        /// <summary>
        /// Тип действия
        /// </summary>
        public RegistryActionEnum Action { get;private set; }
    }
}
