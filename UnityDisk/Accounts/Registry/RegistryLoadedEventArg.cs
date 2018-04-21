using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Accounts.Registry
{
   public class RegistryLoadedEventArg:EventArgs
    {
        public IList<IAccount> Accounts { get; set; }
    }
}
