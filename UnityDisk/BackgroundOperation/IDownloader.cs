using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;

namespace UnityDisk.BackgroundOperation
{
  public  interface IDownloader :IBackgroundOperation
    {
        /// <summary>
        /// Предварительная инициализация после десериализации объекта
        /// <param name="downloaders">Коллекция текущих процессов скачивания</param>
        /// </summary>
        void Initialization(IList<DownloadOperation> downloaders);
    }
}
