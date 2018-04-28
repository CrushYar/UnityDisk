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
   public sealed class SpaceSize
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

        /// <summary>
        /// Прибавляет переданное значение к текущему 
        /// </summary>
        /// <param name="size">Прибавляемое значение</param>
        public void Addition(SpaceSize size)
        {
            TotalSize += size.TotalSize;
            UsedSize += size.UsedSize;
            FreelSize += size.FreelSize;
        }
        /// <summary>
        /// Вычитает переданное значение к текущему
        /// </summary>
        /// <param name="size">Вычитающее значение</param>
        public void Subtraction(SpaceSize size)
        {
            TotalSize -= size.TotalSize;
            UsedSize -= size.UsedSize;
            FreelSize -= size.FreelSize;
        }
        public override Boolean Equals(Object o)
        {
            SpaceSize other = o as SpaceSize;
            if (other == null) return false;
            return other.FreelSize == FreelSize && other.TotalSize == TotalSize && other.UsedSize == UsedSize;
        }
        public Boolean Equals(SpaceSize other)
        {
            return other.FreelSize == FreelSize && other.TotalSize == TotalSize && other.UsedSize == UsedSize;
        }
    }
}
