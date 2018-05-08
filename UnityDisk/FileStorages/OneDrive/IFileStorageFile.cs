using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages.OneDrive
{
    /// <summary>
    /// Интерфейс файла OneDrive
    /// </summary>
    public interface IFileStorageFile: FileStorages.IFileStorageFile,FileStorages.OneDrive.IFileStorageItem
    {
       
    }
}
