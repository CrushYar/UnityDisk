using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Accounts.Registry
{
    public class AccountProjection:IAccountProjection
    {
        public string Login { get; set; }
        public DateTime CreateDate { get; set; }
        public SpaceSize Size { get; set; }
        public string ServerName { get; set; }
        public string Token { get; set; }
        public bool IsFree => Groups.Count == 0;
        public IList<string> Groups { get; private set; }

        public AccountProjection()
        {
            Groups=new List<string>();
        }
        public AccountProjection(IAccount account)
        {
            Login = account.Login;
            CreateDate = account.CreateDate;
            Size = new SpaceSize(account.Size);
            ServerName = account.ServerName;
            Token = account.Token;
            Groups = new List<string>(account.Groups);
        }

        public void SetDataContext(IAccount account)
        {
            Login = account.Login;
            CreateDate = account.CreateDate;
            Size = new SpaceSize(account.Size);
            ServerName = account.ServerName;
            Token = account.Token;
            Groups = new List<string>(account.Groups);
        }
    }
}
