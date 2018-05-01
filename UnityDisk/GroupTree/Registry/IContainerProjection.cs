using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.GroupTree.Registry
{
    public interface IContainerProjection:IGroupTreeItemProjection
    {
        /// <summary>
        /// Режим контейнера
        /// </summary>
        bool IsActive { get; }
        /// <summary>
        /// Устанавливает контекст данных
        /// </summary>
        /// <param name="container">Источник данных</param>
        void SetDataContext(IContainer container);
        /// <summary>
        /// Получение списка дочерних элементов
        /// </summary>
        /// <returns></returns>
        List<IGroupTreeItemProjection> GetChildren();
    }
}
