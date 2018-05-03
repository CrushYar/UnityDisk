using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.StorageItems.PreviewImageManager
{
    /// <summary>
    /// Базовый интерфейс менеджера по загрузке изображений предварительного просмотра
    /// </summary>
    public interface ILoadPreviewImageManager
    {
        /// <summary>
        /// Загрузка изображений предварительного просмотра
        /// </summary>
        /// <param name="items">Колекция элементов для которых нужно произвести загрузку</param>
        void LoadPreviewImages(IList<IStorageItem> items);
        /// <summary>
        /// Сбрасывание всех загружаемых изображений предварительного просмотра
        /// </summary>
        void Reset();
    }
}
