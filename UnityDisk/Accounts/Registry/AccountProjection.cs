using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Accounts.Registry
{
    public class AccountProjection:IAccountProjection
    {
        private IAccount _originAccount;

        public string Login => _originAccount.Login;
        public DateTime CreateDate => _originAccount.CreateDate;
        public SpaceSize Size =>new SpaceSize(_originAccount.Size);
        public string ServerName => _originAccount.ServerName;
        public string Token => _originAccount.Token;
        public bool IsFree => Groups.Count == 0;
        public IList<string> Groups => new List<string>(_originAccount.Groups);

        public AccountProjection()
        {
        }
        public AccountProjection(IAccount account)
        {
            SetDataContext(account);
        }

        public void SetDataContext(IAccount account)
        {
            _originAccount = account;
        }
    }
}
