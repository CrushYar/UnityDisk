using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace UnityDisk.StorageItems.PreviewImageManager
{
    /// <summary>
    /// Реестр стандартных изображений предварительного просмотра
    /// </summary>
    public interface IStandardPreviewImagesRegistry
    {
        /// <summary>
        /// Получение стандартного изображения
        /// </summary>
        /// <param name="type">Тип элемента</param>
        /// <returns></returns>
        BitmapImage FindPreviewImage(StorageItemTypeEnum type);
        /// <summary>
        /// Регистрация стандартного изображения
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <param name="type">Тип элемента</param>
        void Registry(BitmapImage image, StorageItemTypeEnum type);
    }
}
