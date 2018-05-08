using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages.OneDrive
{
    public class FileStorageFactory:IFileStorageFactory
    {
        public FileStorages.IFileStorageAccount CreateAccount()
        {
            return new OneDrive.Account();
        }

        public FileStorages.IFileStorageFolder CreateFolder()
        {
            return  new OneDrive.FileStorageFolder();
        }

        public FileStorages.IFileStorageFile CreateFile()
        {
            return new OneDrive.FileStorageFile();
        }
    }
}
