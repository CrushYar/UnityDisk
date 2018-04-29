﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnityDisk.Settings.Groups
{
    [XmlInclude(typeof(GroupSettingsContainer))]
    [XmlInclude(typeof(GroupSettingsGroup))]
    public abstract class GroupSettingsItem
    {
        public abstract string Name { get; set; }
    }
}
