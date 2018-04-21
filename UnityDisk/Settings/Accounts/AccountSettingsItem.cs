using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Settings.Accounts
{
    public class AccountSettingsItem : IAccountSettingsItem
    {
        public string Login { get; set; }
        public string Token { get; set; }
        public string ServerName { get; set; }
    }
}
