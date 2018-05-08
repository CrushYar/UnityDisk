using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages.OneDrive
{
    /// <summary>
    /// Базовый интерфейс элемента файловой системы OneDrive
    /// </summary>
    public interface IFileStorageItem
    {
        /// <summary>
        /// ID файла
        /// </summary>
        string Id { get; set; }
    }
}
