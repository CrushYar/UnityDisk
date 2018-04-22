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

        public SpaceSize(SpaceSize size)
        {
            this.TotalSize = size.TotalSize;
            this.UsedSize = size.UsedSize;
            this.FreelSize = size.FreelSize;
        }

        public SpaceSize()
        {

        }
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
