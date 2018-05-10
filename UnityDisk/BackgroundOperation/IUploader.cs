using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;

namespace UnityDisk.BackgroundOperation
{
    public interface IUploader:IBackgroundOperation
    {
        /// <summary>
        /// Предварительная инициализация после десериализации объекта
        /// <param name="uploaders">Коллекция текущих процессов загрузки</param>
        /// </summary>
        void Initialization(IList<UploadOperation> uploaders);
    }
}
