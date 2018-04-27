using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Settings.Accounts
{
    public interface IAccountSettingsItem: IEquatable<IAccountSettingsItem>,IComparable<IAccountSettingsItem>
    {
        string Login { get; set; }
        string Token { get; set; }
        string ServerName { get; set; }
    }
}
