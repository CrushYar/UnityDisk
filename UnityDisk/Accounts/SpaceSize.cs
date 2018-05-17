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
            this.FreeSize = size.FreeSize;
        }

        public SpaceSize()
        {

        }
        /// <summary>
        /// Общий размер
        /// </summary>
        public ulong TotalSize { get; set; }
        /// <summary>
        /// Используемый размер
        /// </summary>
        public ulong UsedSize { get; set; }
        /// <summary>
        /// Свободный размер
        /// </summary>
        public ulong FreeSize { get; set; }

        /// <summary>
        /// Прибавляет переданное значение к текущему 
        /// </summary>
        /// <param name="size">Прибавляемое значение</param>
        public void Addition(SpaceSize size)
        {
            TotalSize += size.TotalSize;
            UsedSize += size.UsedSize;
            FreeSize += size.FreeSize;
        }
        /// <summary>
        /// Вычитает переданное значение к текущему
        /// </summary>
        /// <param name="size">Вычитающее значение</param>
        public void Subtraction(SpaceSize size)
        {
            TotalSize -= size.TotalSize;
            UsedSize -= size.UsedSize;
            FreeSize -= size.FreeSize;
        }
        public override Boolean Equals(Object o)
        {
            SpaceSize other = o as SpaceSize;
            if (other == null) return false;
            return other.FreeSize == FreeSize && other.TotalSize == TotalSize && other.UsedSize == UsedSize;
        }
        public Boolean Equals(SpaceSize other)
        {
            return other.FreeSize == FreeSize && other.TotalSize == TotalSize && other.UsedSize == UsedSize;
        }

        public override int GetHashCode()
        {
           return Tuple.Create(TotalSize, UsedSize, FreeSize).GetHashCode();
        }
    }
}
