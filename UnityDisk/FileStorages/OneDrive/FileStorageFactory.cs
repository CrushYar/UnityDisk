using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.BackgroundOperation;

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

        public IBackgroundOperation ParseBackgroundOperation(BackgroundOperationActionEnum action, string data)
        {
            switch (action)
            {
                case BackgroundOperationActionEnum.Download:
                    return OneDrive.Downloader.Parse(data);
                case BackgroundOperationActionEnum.Upload:
                    return OneDrive.Uploader.Parse(data);
                default:
                    throw new ArgumentException("Unknown type");
            }
        }
    }
}
