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
        public IList<IAccountProjection> Items { get;  set; }
        public string Name { get; set; }
        public SpaceSize Size { get; set; }

        public GroupTreeTypeEnum Type=>GroupTreeTypeEnum.Group;

        public IContainer Parent { get; set; }

        public Group()
        {
            Items=new List<IAccountProjection>();
        }

        public IGroupTreeItem Clone()
        {
            var clone=new Group();
            clone.Size = new SpaceSize(Size);
            clone.Items=new List<IAccountProjection>(Items);
            clone.Parent = Parent;
            return clone;
        }

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

            Size = size;
        }

        public List<IAccountProjection> GetAccountProjections()
        {
            return new List<IAccountProjection>(Items); ;
        }
    }
}
