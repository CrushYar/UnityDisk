using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnityDisk.Settings.Groups
{
    public sealed class GroupSettingsContainer: GroupSettingsContainerTemplate
    {
        public override string Name { get; set; }
        public override bool IsActive { get; set; }
        public override List<GroupSettingsItem> Items { get; set; }
    }
}
