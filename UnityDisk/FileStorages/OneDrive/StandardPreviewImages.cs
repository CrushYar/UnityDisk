using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using UnityDisk.StorageItems.PreviewImageManager;

namespace UnityDisk.FileStorages.OneDrive
{
    /// <summary>
    /// Класс по поиску картинки предварительного просмотра по типу элемента OneDrive
    /// </summary>
    public class StandardPreviewImages:FileStorages.IStandardPreviewImages
    {
        private StorageItems.PreviewImageManager.IStandardPreviewImagesRegistry _imagesRegistry;
        private readonly Dictionary<string, BitmapImage> _hashImages=new Dictionary<string, BitmapImage>();
        private SpinLock _spinLock = new SpinLock(true);

        public StandardPreviewImages()
        {
            _imagesRegistry=new StandardPreviewImagesRegistry();
        }

        public StandardPreviewImages(StorageItems.PreviewImageManager.IStandardPreviewImagesRegistry imagesRegistry)
        {
            _imagesRegistry = imagesRegistry;
        }
        public BitmapImage FindPreviewImageByType(string type)
        {
            Lock();
            _hashImages.TryGetValue(type, out BitmapImage image);
            UnLock();

            if (image != null) return image;

            Regex regex=new Regex(@"^text/*");
            if (regex.IsMatch(type))
            {
            }

            throw new FileNotFoundException("Image has not found");
        }

        private void AddNewType(string type, BitmapImage image)
        {
            Lock();
            _hashImages.Add(type,image);
            UnLock();
        }
        private void Lock()
        {
            bool taken = false;
            _spinLock.Enter(ref taken);
        }
        private void UnLock()
        {
            _spinLock.Exit();
        }
    }
}
