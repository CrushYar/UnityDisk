﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.Settings.Accounts
{
    public sealed class AccountSettingsItem : IAccountSettingsItem
    {
        public AccountSettingsItem() { }
        public AccountSettingsItem(IAccount account)
        {
            this.Login = account.Login;
            this.Token = account.Token;
            this.ServerName = account.ServerName;
        }

        public string Login { get; set; }
        public string Token { get; set; }
        public string ServerName { get; set; }

        public int CompareTo(IAccountSettingsItem other)
        {
            int result = Login.CompareTo(other.Login);
            if (result != 0) return result;
            result = Token.CompareTo(other.Token);
            if (result != 0) return result;
            return ServerName.CompareTo(other.ServerName);
        }

        public bool Equals(IAccountSettingsItem other)
        {
            return Login.Equals(other.Login) && Token.Equals(other.Token) && ServerName.Equals(other.ServerName);
        }

        public override Boolean Equals(Object o)
        {
            IAccountSettingsItem other = o as IAccountSettingsItem;
            if (other == null) return false;
            return Login.Equals(other.Login) && Token.Equals(other.Token) && ServerName.Equals(other.ServerName);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Login,Token,ServerName).GetHashCode();
        }
    }
}
