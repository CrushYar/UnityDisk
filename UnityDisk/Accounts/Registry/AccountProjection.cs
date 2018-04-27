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
        public bool IsFree { get; }
        public IList<string> Groups { get; }
    }
}
