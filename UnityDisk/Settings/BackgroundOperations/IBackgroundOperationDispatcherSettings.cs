using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Settings.BackgroundOperations
{
    /// <summary>
    /// Базовый интерфейс для сохранения и загрузки коллекций фоновых операций скачивания/загрузки
    /// </summary>
    public interface IBackgroundOperationDispatcherSettings
    {
        /// <summary>
        /// Сохранение фоновых операций
        /// </summary>
        /// <param name="paramName">Имя под которым будет сохраняться коллекция</param>
        /// <param name="backgroundOperations">Коллекция операций, которую нужно сохранить</param>
        void SaveOperations(string paramName,
            IList<UnityDisk.BackgroundOperation.IBackgroundOperation> backgroundOperations);
        /// <summary>
        /// Загрузка коллекции фоновых операцих
        /// </summary>
        /// <param name="paramName">Имя параметра из которого нужно загрузить</param>
        /// <returns>Загруженная коллеккция</returns>
        IList<UnityDisk.BackgroundOperation.IBackgroundOperation> LoadOperations(string paramName);
    }
}
