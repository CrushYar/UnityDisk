using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using UnityDisk.Accounts;

namespace UnityDisk.StorageItems.PreviewImageManager
{
    public class StandardPreviewImagesRegistry:IStandardPreviewImagesRegistry
    {
        private readonly System.Collections.Generic.Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();
        /// <summary>
        /// Объект синхронизации
        /// </summary>
        private SpinLock _spinLock = new SpinLock(true);
        public StandardPreviewImagesRegistry()
        {
        }
        public BitmapImage FindPreviewImage(string type)
        {
            Lock();
            _images.TryGetValue(type, out BitmapImage value);
            UnLock();
            return value;
        }

        public void Registry(BitmapImage image, string type)
        {
            Lock();
            if(_images.ContainsKey(type))throw new ArgumentException("This type has already registered");
            _images.Add(type,image);
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
