using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages.OneDrive
{
    public class FileStorageFactory:IFileStorageFactory
    {
        public IFileStorageAccount CreateAccount()
        {
            return new OneDrive.Account();
        }

        public IFileStorageFolder CreateFolder()
        {
            throw new NotImplementedException();
        }

        public IFileStorageFile CreateFile()
        {
            throw new NotImplementedException();
        }
    }
}
