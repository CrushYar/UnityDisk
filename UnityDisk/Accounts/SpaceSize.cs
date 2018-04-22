using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Accounts
{
    /// <summary>
    /// Тип размера пространства
    /// </summary>
   public class SpaceSize
    {
        /// <summary>
        /// Общий размер
        /// </summary>
        public long TotalSize { get; set; }
        /// <summary>
        /// Используемый размер
        /// </summary>
        public long UsedSize { get; set; }
        /// <summary>
        /// Свободный размер
        /// </summary>
        public long FreelSize { get; set; }
    }
}
