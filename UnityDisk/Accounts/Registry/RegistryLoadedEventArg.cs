using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Accounts.Registry
{
   public sealed class RegistryLoadedEventArg:EventArgs
    {
        public SpaceSize Size { get; set; }
        public IAccount[] Accounts { get; set; }
    }
}
