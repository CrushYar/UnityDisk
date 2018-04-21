using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Accounts
{
    /// <summary>
    /// Параметр события об изменении размера
    /// </summary>
    public class SizeChangedEventArg : EventArgs
    {
        /// <summary>
        /// Старый размер
        /// </summary>
        public SpaceSize OldSize { get; set; }
        /// <summary>
        /// Новый размер
        /// </summary>
        public SpaceSize NewSize { get; set; }
    }
}
