using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityDisk.GroupTree;

namespace UnityDisk.Settings.Groups
{
    public sealed class GroupSettingsContainer:GroupSettingsItem, IEquatable<GroupSettingsContainer>
    {
        public override string Name { get; set; }
        public bool IsActive { get; set; }
        public List<GroupSettingsItem> Items { get; set; }
        [XmlIgnore]
        public override GroupTreeTypeEnum Type => GroupTreeTypeEnum.Container;

        public GroupSettingsContainer() { }
        public GroupSettingsContainer(IContainer container)
        {
            Name = container.Name;
            IsActive = container.IsActive;
            Items = new List<GroupSettingsItem>();
            foreach (var item in container.Items)
            {
                switch (item)
                {
                    case IContainer nextContainer:
                        Items.Add(new GroupSettingsContainer(nextContainer));
                        break;
                    case IGroup nextGroup:
                        Items.Add(new GroupSettingsGroup(nextGroup));
                        break;
                }
            }
        }
        public bool Equals(GroupSettingsContainer other)
        {
            if (other == null) return false;
            if (Name != other.Name) return false;
            if (Items == other.Items) return true;
            if ((Items == null && other.Items != null)
                ||
                (Items != null && other.Items == null)) return false;
            return Items.OrderBy(kvp => kvp.Name, StringComparer.Ordinal)
                .SequenceEqual(other.Items.OrderBy(kvp => kvp.Name, StringComparer.Ordinal));
        }
        public override bool Equals(object obj)
        {
            var container = obj as GroupSettingsContainer;
            if (container == null) return false;
            if (Name != container.Name) return false;
            if (Items == container.Items) return true;
            if ((Items == null && container.Items != null)
                ||
                (Items != null && container.Items == null)) return false;
            return Items.OrderBy(kvp => kvp.Name, StringComparer.Ordinal)
                .SequenceEqual(container.Items.OrderBy(kvp => kvp.Name, StringComparer.Ordinal));
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Name,IsActive,Items).GetHashCode();
        }
    }
}
