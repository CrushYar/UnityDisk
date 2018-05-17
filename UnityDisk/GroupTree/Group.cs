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
            clone.Name = Name;
            clone.Size = new SpaceSize(Size);
            clone.Items=new List<IAccountProjection>(Items);
            clone.Parent = Parent;
            return clone;
        }

        public Task<FileStorages.IFileStorageFolder> LoadDirectory()
        {
            throw new NotImplementedException();
        }

        public void LoadSizeInfo()
        {
            if (Size == null)
                Size = new SpaceSize();
            else
                Size.TotalSize = Size.UsedSize = Size.FreeSize = 0;

            foreach (var item in Items)
            {
                Size.TotalSize += item.Size.TotalSize;
                Size.UsedSize += item.Size.UsedSize;
                Size.FreeSize += item.Size.FreeSize;
            }
        }

        public List<IAccountProjection> GetAccountProjections()
        {
            return new List<IAccountProjection>(Items); ;
        }
    }
}
