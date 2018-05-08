using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace UnityDisk.FileStorages
{
    /// <summary>
    /// Базовый интерфейс для получени картинки предварительного просмотра относительно типа элемента определенного облака
    /// </summary>
   public interface IStandardPreviewImages
   {
       /// <summary>
       /// Поиск картинки предварительного просмотра относительно типа элемента
       /// </summary>
       /// <param name="type">Тип элемента</param>
       /// <returns>Картинка предварительного просмотра</returns>
       BitmapImage FindPreviewImageByType(string type);
   }
}
