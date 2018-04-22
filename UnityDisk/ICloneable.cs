using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk
{
    /// <summary>
    /// Интерфейс позволяющий делать клонирование
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneable<T>
    {
        /// <summary>
        /// Возвращает копию своего объекта
        /// </summary>
        /// <returns>Копия объекта</returns>
        T Clone();
    }
}
