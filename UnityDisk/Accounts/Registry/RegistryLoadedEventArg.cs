using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Accounts.Registry
{
   public class RegistryLoadedEventArg:EventArgs
    {
        public IAccount[] Accounts { get; set; }
    }
}
