using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityDisk.GroupTree;

namespace UnityDisk.Settings.Groups
{
    public sealed class GroupSettingsGroup : GroupSettingsItem, IEquatable<GroupSettingsGroup>
    {
        public GroupSettingsGroup(IGroup group)
        {
            Name = group.Name;
            Items = new List<string>();

            foreach (var item in group.Items)
            {
                Items.Add(item.Login);
            }
        }

        public override string Name { get; set; }
        public List<string> Items { get; set; }
        [XmlIgnore]
        public override GroupTreeTypeEnum Type=> GroupTreeTypeEnum.Group;

        public bool Equals(GroupSettingsGroup other)
        {
            if (other == null) return false;
            if (Name != other.Name) return false;
            if (Items == other.Items) return true;
            if ((Items == null && other.Items != null)
                ||
                (Items != null && other.Items == null)) return false;
            return Items.OrderBy(kvp => kvp, StringComparer.Ordinal)
                .SequenceEqual(other.Items.OrderBy(kvp => kvp, StringComparer.Ordinal));
        }

        public override bool Equals(object obj)
        {
            var group = obj as GroupSettingsGroup;
            if (group == null) return false;
            if (Name != group.Name) return false;
            if (Items == group.Items) return true;
            if ((Items == null && group.Items != null)
                ||
                (Items != null && group.Items == null)) return false;
            return Items.OrderBy(kvp => kvp, StringComparer.Ordinal)
                .SequenceEqual(group.Items.OrderBy(kvp => kvp, StringComparer.Ordinal));
        }
    }
}
