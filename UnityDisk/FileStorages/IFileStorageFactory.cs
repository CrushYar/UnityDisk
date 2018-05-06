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
        IFileStorageFolder CreateFolder(string serverName);
        IFileStorageFile CreateFile(string serverName);
    }
}
