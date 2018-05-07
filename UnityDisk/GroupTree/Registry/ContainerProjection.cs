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

        public bool Equals(IGroupTreeItemProjection projection)
        {
            var container = projection as ContainerProjection;
            if (container == null) return false;

            if (Name != container.Name) return false;
            if (Type != container.Type) return false;
            if (IsActive != container.IsActive) return false;
            if (!Size.Equals(container.Size)) return false;

            var myChildren = GetChildren();
            var otheChildren= container.GetChildren();
            if ((myChildren == null && otheChildren != null) ||
                (myChildren != null && otheChildren == null)) return false;
            if (myChildren == null) return true;

            if (myChildren.Count != otheChildren.Count) return false;
            var myEnum = myChildren.GetEnumerator();
            var otherEnum = otheChildren.GetEnumerator();

            bool canMyEnumNext = myEnum.MoveNext();
            bool canOtherEnumNext = otherEnum.MoveNext();
            if (canOtherEnumNext != canMyEnumNext) return false;

            while (canMyEnumNext)
            {
                if (!myEnum.Current.Equals(otherEnum.Current)) return false;

                canMyEnumNext = myEnum.MoveNext();
                canOtherEnumNext = otherEnum.MoveNext();
                if (canOtherEnumNext != canMyEnumNext) return false;
            }
            return true;
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
            return _originContainer.GetAccountProjections();
        }
        public Task<IList<FileStorages.IFileStorageItem>> LoadDirectory()
        {
            return _originContainer.LoadDirectory();
        }
    }
}
