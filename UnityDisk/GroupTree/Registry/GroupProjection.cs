using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.GroupTree.Registry
{
    public class GroupProjection : IGroupProjection
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
    }
}
