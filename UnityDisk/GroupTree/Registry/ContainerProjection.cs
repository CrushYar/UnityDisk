using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

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
    }
}
