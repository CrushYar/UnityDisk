using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UnityDisk.Settings.Accounts
{
    public interface IAccountSettingsItem
    {
        string Login { get; set; }
        string Token { get; set; }
        string ServerName { get; set; }
    }
}
