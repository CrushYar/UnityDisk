using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages;

namespace UnityDisk.GroupTree.Registry
{
    public sealed class GroupProjection : IGroupProjection
    {
        private IGroup _originGroup;
        public string Name => _originGroup.Name;

        public SpaceSize Size => new SpaceSize(_originGroup.Size);

        public GroupTreeTypeEnum Type =>GroupTreeTypeEnum.Group;

        public GroupProjection() { }

        public GroupProjection(IGroup group)
        {
            SetDataContext(group);
        }
        public void SetDataContext(IGroup group)
        {
            _originGroup = group;
        }

        public IGroupTreeItemProjection Clone()
        {
            return new GroupProjection(_originGroup);
        }

        public List<IAccountProjection> GetAccountProjections()
        {
            return _originGroup.GetAccountProjections();
        }

        public Task<IList<IFileStorageItem>> LoadDirectory()
        {
            return _originGroup.LoadDirectory();
        }

        public bool Equals(IGroupTreeItemProjection projection)
        {
            var otherGroup = projection as GroupProjection;
            if (otherGroup == null) return false;

            if (Name != otherGroup.Name) return false;
            if (Type != otherGroup.Type) return false;
            if (!Size.Equals(otherGroup.Size)) return false;

            var myAccounts= GetAccountProjections();
            var otheAccounts = otherGroup.GetAccountProjections();

            if ((myAccounts == null && otheAccounts != null) ||
                (myAccounts != null && otheAccounts == null)) return false;
            if (myAccounts == null) return true;

            if (myAccounts.Count != otheAccounts.Count) return false;
            var myEnum = myAccounts.GetEnumerator();
            var otherEnum = otheAccounts.GetEnumerator();

            bool canMyEnumNext = myEnum.MoveNext();
            bool canOtherEnumNext = otherEnum.MoveNext();
            if (canOtherEnumNext != canMyEnumNext) return false;

            while (canMyEnumNext)
            {
                if (myEnum.Current.Login!=otherEnum.Current.Login) return false;

                canMyEnumNext = myEnum.MoveNext();
                canOtherEnumNext = otherEnum.MoveNext();
                if (canOtherEnumNext != canMyEnumNext) return false;
            }
            return true;
        }
    }
}
