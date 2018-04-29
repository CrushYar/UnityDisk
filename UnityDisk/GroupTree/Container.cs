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

        public Container()
        {
            Items=new List<IGroupTreeItem>();
        }
        public IGroupTreeItem Clone()
        {
            var clone=new Container();
            clone.Items=new List<IGroupTreeItem>(Items.Count);
            foreach (var item in Items)
            {
                clone.Items.Add(item.Clone());
            }
            clone.Name = Name;
            clone.IsActive = IsActive;
            clone.Size = new SpaceSize(Size);
            clone.Parent = Parent;

            return clone;
        }

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
