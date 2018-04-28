using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.GroupTree
{
    public sealed class Container : IContainer
    {
        private SpaceSize _size;

        public IList<IGroupTreeItem> Items { get; set; }
        public string Name { get; set; }
        public SpaceSize Size => new SpaceSize(_size);

        public GroupTreeTypeEnum Type => GroupTreeTypeEnum.Container;

        public bool IsActive { get; set; }

        public void LoadDirectory()
        {
            throw new NotImplementedException();
        }

        public void LoadSizeInfo()
        {
            SpaceSize size = new SpaceSize();
            foreach (var item in Items)
            {
                item.LoadSizeInfo();
                size.TotalSize += item.Size.TotalSize;
                size.UsedSize += item.Size.UsedSize;
                size.FreelSize += item.Size.FreelSize;
            }
            _size = size;
        }
    }
}
