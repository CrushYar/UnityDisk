using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.GroupTree
{
    /// <summary>
    /// Параметр возвращаемый событием после переименования элемента дерева групп
    /// </summary>
    public sealed class RenameEventArg:EventArgs
    {
        /// <summary>
        /// Старое название
        /// </summary>
        public string OldName { get; set; }
        /// <summary>
        /// Новое название
        /// </summary>
        public string NewName { get; set; }
        /// <summary>
        /// Элемент имя которого было изменено
        /// </summary>
        public IGroupTreeItem item { get; set; }
    }
}
