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
        Image       = 6,
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
        void Rename(string newName);
        /// <summary>
        /// Удаление элемента
        /// </summary>
        void Delete();
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="folder">Целевая папка</param>
        void Move(IStorageProjectionFolder folder);
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="folder">Целевая папка</param>
        void Copy(IStorageProjectionFolder folder);
        /// <summary>
        /// Загрузка картинки предварительно просмотра
        /// </summary>
        void LoadPreviewImage();
        /// <summary>
        /// Загрузка публичной ссылки
        /// </summary>
        void LoadPublicUrl();
        /// <summary>
        /// Создание публичной ссылки
        /// </summary>
        void CreatePublicUrl();
        /// <summary>
        /// Удаление публичной ссылки
        /// </summary>
        void DeletePublicUrl();
    }
}
