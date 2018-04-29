using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Settings.Groups
{
    public interface IGroupSettings
    {
        GroupSettingsContainer LoadGroupTree();
        void SaveGroupTree();
    }
}
