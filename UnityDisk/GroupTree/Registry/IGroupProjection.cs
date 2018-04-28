using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.GroupTree.Registry
{
    public interface IGroupProjection:IGroupTreeItemProjection
    {
        /// <summary>
        /// Устанавливает контекст данных
        /// </summary>
        /// <param name="group">Источник данных</param>
        void SetDataContext(IGroup group);
    }
}
