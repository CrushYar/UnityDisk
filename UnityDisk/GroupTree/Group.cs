using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.GroupTree
{
    public sealed class Group : IGroup
    {
        private SpaceSize _size;
        public IList<IAccount> Items { get; private set; }
        public string Name { get; set; }
        public SpaceSize Size => new SpaceSize(_size);

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
