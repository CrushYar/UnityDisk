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

        public IList<IGroupTreeItem> Items { get; private set; }
        public string Name { get; set; }
        public SpaceSize Size => new SpaceSize(_size);

        public event EventHandler<ContainerLoadedEventArg> LoadedEvent;
        public event EventHandler<ContainerUnloadedEventArg> UnloadEvent;

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

        private void OnUnload()
        {
            UnloadEvent?.Invoke(this, new ContainerUnloadedEventArg(Items));
        }
        private void Onload()
        {
            LoadedEvent?.Invoke(this, new ContainerLoadedEventArg(Items));
        }
    }
}
