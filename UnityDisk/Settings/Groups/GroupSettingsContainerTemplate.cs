﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnityDisk.Settings.Groups
{
    [XmlInclude(typeof(GroupSettingsContainer))]
    public abstract class GroupSettingsContainerTemplate : GroupSettingsItem,IEquatable<GroupSettingsContainerTemplate>
    {
       public abstract bool IsActive { get; set; }
        public abstract List<GroupSettingsItem> Items { get; set; }

        public bool Equals(GroupSettingsContainerTemplate other)
        {
            if (other == null) return false;
            if (Name!=other.Name) return false;
            if (Items == other.Items) return true;
            if ((Items == null && other.Items != null)
                ||
                (Items != null && other.Items == null)) return false;
            return Items.OrderBy(kvp => kvp.Name, StringComparer.Ordinal)
                .SequenceEqual(other.Items.OrderBy(kvp => kvp.Name, StringComparer.Ordinal));
        }
        public override bool Equals(object obj)
        {
            var container = obj as GroupSettingsContainerTemplate;
            if (container == null) return false;
            if (Name!=container.Name) return false;
            if (Items == container.Items) return true;
            if ((Items == null && container.Items!=null)
                ||
                (Items != null && container.Items == null)) return false;
            return Items.OrderBy(kvp => kvp.Name, StringComparer.Ordinal)
                .SequenceEqual(container.Items.OrderBy(kvp => kvp.Name, StringComparer.Ordinal));
        }
    }
}
