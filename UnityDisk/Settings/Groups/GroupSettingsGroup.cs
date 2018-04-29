using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnityDisk.Settings.Groups
{
    public sealed class GroupSettingsGroup : GroupSettingsItem, IEquatable<GroupSettingsGroup>
    {
        public override string Name { get; set; }
        public List<string> Items { get; set; }
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
