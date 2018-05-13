using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages
{
    public interface IFileStorageFactory
    {
        FileStorages.IFileStorageAccount CreateAccount();
        FileStorages.IFileStorageFolder CreateFolder();
        FileStorages.IFileStorageFile CreateFile();
        BackgroundOperation.IBackgroundOperation ParseBackgroundOperation(
            BackgroundOperation.BackgroundOperationActionEnum action, string data);
    }
}
