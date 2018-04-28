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
        public IList<IGroupTreeItem> Items { get; set; }
        public string Name { get; set; }
        public SpaceSize Size { get; set; }
        public GroupTreeTypeEnum Type => GroupTreeTypeEnum.Container;

        public bool IsActive { get; set; }
        public IContainer Parent { get; set; }

        public void LoadDirectory()
        {
            throw new NotImplementedException();
        }

        public void LoadSizeInfo()
        {
            Size = new SpaceSize();
            foreach (var item in Items)
            {
                item.LoadSizeInfo();
                Size.TotalSize += item.Size.TotalSize;
                Size.UsedSize += item.Size.UsedSize;
                Size.FreelSize += item.Size.FreelSize;
            }
        }
    }
}
