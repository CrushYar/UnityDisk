using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.GroupTree.Registry
{
    public class ContainerProjection : IContainerProjection
    {
        private IContainer _container;

        public string Name => _container.Name;

        public SpaceSize Size => new SpaceSize(_container.Size);

        public GroupTreeTypeEnum Type => GroupTreeTypeEnum.Container;

        public bool IsActive => _container.IsActive;

        public ContainerProjection() { }
        public ContainerProjection(IContainer container)
        {
            SetDataContext(container);
        }

        public void SetDataContext(IContainer container)
        {
            _container = container;
        }
    }
}
