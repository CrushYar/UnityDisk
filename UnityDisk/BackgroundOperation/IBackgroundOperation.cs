using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnityDisk.BackgroundOperation
{
    public enum BackgroundOperationActionEnum
    {
        Download, Upload
    }
    public enum BackgroundOperationStateEnum
    {
        Waiting, Running,Completed,Error
    }
    /// <summary>
    /// Базовый интерфейс элементов фоновых операций по передаче данных
    /// </summary>
    public interface IBackgroundOperation
    {
        /// <summary>
        /// Общее количество байт необходимых для передачи
        /// </summary>
        ulong TotalBytesToTransfer { get; }
        /// <summary>
        /// Количество переданных байт
        /// </summary>
        ulong ByteTransferred { get; }
        /// <summary>
        /// Скорость передачи байтов
        /// </summary>
        ulong Speed { get; }
        /// <summary>
        /// Тип операции
        /// </summary>
        BackgroundOperationActionEnum Action { get; }
        /// <summary>
        /// Дата завершения операции
        /// </summary>
        DateTime DateCompleted { get; }
        /// <summary>
        /// Статус операции
        /// </summary>
        BackgroundOperationStateEnum State { get; }
        /// <summary>
        /// Файл облачного сервиса
        /// </summary>
        UnityDisk.StorageItems.IStorageFile RemoteFile { get; }
        /// <summary>
        /// Начать фоновую операцию
        /// </summary>
        /// <returns></returns>
        Task Start();
        /// <summary>
        /// Остановка фоновой операции
        /// </summary>
        /// <returns></returns>
        void Stop();
        /// <summary>
        /// Выполнить шаг перерасчета скорости и скачанных байт
        /// </summary>
        /// <returns></returns>
        void Step();
        /// <summary>
        /// Предварительная инициализация после десериализации объекта
        /// </summary>
        void Initialization();
    }
}
