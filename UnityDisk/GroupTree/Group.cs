using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;

namespace UnityDisk.GroupTree
{
    public sealed class Group : IGroup
    {
        private SpaceSize _size;
        public IList<IAccountProjection> Items { get; private set; }
        public string Name { get; set; }
        public SpaceSize Size => new SpaceSize(_size);

        public GroupTreeTypeEnum Type=>GroupTreeTypeEnum.Group;

        public void LoadDirectory()
        {
            throw new NotImplementedException();
        }

        public void LoadSizeInfo()
        {
            SpaceSize size = new SpaceSize();
            foreach (var item in Items)
            {
                size.TotalSize += item.Size.TotalSize;
                size.UsedSize += item.Size.UsedSize;
                size.FreelSize += item.Size.FreelSize;
            }

            _size = size;
        }
    }
}
