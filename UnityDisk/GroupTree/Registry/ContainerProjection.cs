using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;

namespace UnityDisk.GroupTree.Registry
{
    public sealed class ContainerProjection : IContainerProjection
    {
        private IContainer _originContainer;

        public string Name => _originContainer.Name;

        public SpaceSize Size => new SpaceSize(_originContainer.Size);

        public GroupTreeTypeEnum Type => GroupTreeTypeEnum.Container;

        public bool IsActive => _originContainer.IsActive;

        public ContainerProjection() { }
        public ContainerProjection(IContainer container)
        {
            SetDataContext(container);
        }

        public void SetDataContext(IContainer container)
        {
            _originContainer = container;
        }

        public IGroupTreeItemProjection Clone()
        {
            return new ContainerProjection(_originContainer);
        }

        public List<IGroupTreeItemProjection> GetChildren()
        {
            List<IGroupTreeItemProjection> result=new List<IGroupTreeItemProjection>(_originContainer.Items.Count);
            foreach (var item in _originContainer.Items)
            {
                switch (item)
                {
                    case IGroup group:
                        result.Add(new GroupProjection(group));
                        break;
                    case IContainer container:
                        result.Add(new ContainerProjection(container));
                        break;
                    default:
                        throw new ArgumentException("Unknown type");
                }
            }

            return result;
        }

        public List<IAccountProjection> GetAccountProjections()
        {
            List<IAccountProjection> result=new List<IAccountProjection>();

            GetChildren(_originContainer, result);
            return result;
        }

        private void GetChildren(IGroupTreeItem item, List<IAccountProjection> result)
        {
            switch (item)
            {
                case IGroup group:
                    foreach (var groupAccount in group.Items)
                    {
                        if(result.Contains(groupAccount)) continue;

                        result.Add(groupAccount);
                    }
                    break;
                case IContainer container:
                    foreach (var containerItem in container.Items)
                    {
                        GetChildren(containerItem, result);
                    }
                    break;
                default:
                    throw new ArgumentException("Unknown type");
            }
        }
    }
}
