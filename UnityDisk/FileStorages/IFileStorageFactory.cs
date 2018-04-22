using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages
{
    public interface IFileStorageFactory
    {
        IFileStorageAccount CreateAccount(string serverName);
        //IFileStorageAccount CreateFolder(string serverName);
        //IFileStorageAccount CreateFile(string serverName);
    }
}
