using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.BackgroundOperation;

namespace UnityDisk.Settings.BackgroundOperations
{
    [Serializable]
    public class BackgroundOperationSettingsItem
    {
        public BackgroundOperationActionEnum Action { get; set; }
        public string ServerName { get; set; }
        public string State { get; set; }
    }
}
