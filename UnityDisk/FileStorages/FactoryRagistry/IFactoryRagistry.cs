using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages.FactoryRagistry
{
    /// <summary>
    /// Базовый фабричный интерфейс регистрации фабрик для каждого сервера
    /// </summary>
    public interface IFactoryRagistry
    {
        /// <summary>
        /// Регистрация фабрики для указанного сервера
        /// </summary>
        /// <param name="serverName">Имя сервера</param>
        /// <param name="factory">Фабрика сервера</param>
        /// <returns>Успех добавления фабрики</returns>
        bool Registry(string serverName,IFileStorageFactory factory);
        /// <summary>
        /// Создание объекта аккаунта для указанного сервера
        /// </summary>
        /// <param name="serverName">Имя сервера</param>
        /// <returns></returns>
        IFileStorageAccount CreateAccount(string serverName);
        /// <summary>
        /// Создание объекта папки для указанного сервера
        /// </summary>
        /// <param name="serverName">Имя сервера</param>
        /// <returns></returns>
        IFileStorageFolder CreateFolder(string serverName);
        /// <summary>
        /// Создание объекта файла для указанного сервера
        /// </summary>
        /// <param name="serverName">Имя сервера</param>
        /// <returns></returns>
        IFileStorageFile CreateFile(string serverName);
        /// <summary>
        /// Получение объекта управления фоновой операцией скачивания/загрузки из данных строкового типа
        /// </summary>
        /// <param name="action">Тип действия</param>
        /// <param name="data">Данные в строковом виду</param>
        /// <param name="serverName">Имя сервера</param>
        /// <returns>Объекта управления фоновой операцией </returns>
        BackgroundOperation.IBackgroundOperation ParseBackgroundOperation(
            BackgroundOperation.BackgroundOperationActionEnum action, string data,string serverName);
    }
}
