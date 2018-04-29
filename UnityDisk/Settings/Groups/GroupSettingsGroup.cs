using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnityDisk.Settings.Groups
{
    public sealed class GroupSettingsGroup: GroupSettingsGroupTemplate
    {
        public override string Name { get; set; }
        public override List<string> Items { get; set; }
    }
}
