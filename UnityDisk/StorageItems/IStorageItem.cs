using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace UnityDisk.StorageItems
{
    public enum StorageItemAttributeEnum
    {
        Directory   = 2,   
        File        = 4,
    }
    /// <summary>
    /// перечисление состояний элемента файловой системы
    /// </summary>
    public enum StorageItemStateEnum
    {
        Deleted, Exist
    }
    /// <summary>
    /// Базовый интерфейс элемента файловой системы
    /// </summary>
    public interface IStorageItem
    {
        /// <summary>
        /// Имя элемента
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Путь к элементу
        /// </summary>
        string Path { get; }
        /// <summary>
        /// Изображение элементы
        /// </summary>
        BitmapImage PreviewImage { get; set; }
        /// <summary>
        /// Атрибут элемента
        /// </summary>
        StorageItemAttributeEnum Attribute { get; }
        /// <summary>
        /// Состояние элемента
        /// </summary>
        StorageItemStateEnum State { get; }
        /// <summary>
        /// Родительский элемент
        /// </summary>
        IStorageProjectionFolder Parent { get; set; }
        /// <summary>
        /// Переименование элемента
        /// </summary>
        /// <param name="newName">Новое имя</param>
        Task Rename(string newName);
        /// <summary>
        /// Удаление элемента
        /// </summary>
        Task Delete();
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="folder">Целевая папка</param>
        Task Move(IStorageProjectionFolder folder);
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="folder">Целевая папка</param>
        /// <returns>Скопированный файл</returns>
        Task<IStorageItem> Copy(IStorageProjectionFolder folder);
        /// <summary>
        /// Загрузка картинки предварительно просмотра
        /// </summary>
        Task LoadPreviewImage();
        /// <summary>
        /// Загрузка публичной ссылки
        /// </summary>
        Task LoadPublicUrl();
        /// <summary>
        /// Создание публичной ссылки
        /// </summary>
        Task CreatePublicUrl();
        /// <summary>
        /// Удаление публичной ссылки
        /// </summary>
        Task DeletePublicUrl();
    }
}
