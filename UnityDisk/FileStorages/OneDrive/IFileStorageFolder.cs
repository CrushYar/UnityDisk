using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages.OneDrive
{
    /// <summary>
    /// Интерфейс папки OneDrive
    /// </summary>
    public interface IFileStorageFolder:FileStorages.IFileStorageFolder, FileStorages.OneDrive.IFileStorageItem
    {
    }
}
