using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.GroupTree.Registry
{
   public interface IGroupTreeItemProjection
    {
        /// <summary>
        /// Название элемента
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Размер элемента
        /// </summary>
        SpaceSize Size { get; }
        /// <summary>
        /// Тип элемента
        /// </summary>
        GroupTreeTypeEnum Type { get; }
    }
}
