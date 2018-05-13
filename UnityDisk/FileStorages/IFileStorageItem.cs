using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using UnityDisk.Accounts.Registry;
using UnityDisk.StorageItems;

namespace UnityDisk.FileStorages
{
    public interface IFileStorageItem
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
        /// Изображение предварительного просмотра
        /// </summary>
        BitmapImage PreviewImage { get; set; }
        /// <summary>
        /// Тип элемента
        /// </summary>
        StorageItemTypeEnum Type { get; }
        /// <summary>
        /// Публичная ссылка
        /// </summary>
        string PublicUrl { get; }
        /// <summary>
        /// Аккаунт
        /// </summary>
        IAccountProjection Account { get; set; }
        /// <summary>
        /// Дата Создания
        /// </summary>
        DateTime CreateDate { get; }
        /// <summary>
        /// Удаление элемента
        /// </summary>
        Task Delete();
        /// <summary>
        /// Переименование элемента
        /// </summary>
        /// <param name="newName">Новое имя</param>
        Task Rename(string newName);
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="folder">Папка в которую нужно переместить элемент</param>
        Task Move(IFileStorageFolder folder);
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="othePath">Целевой путь</param>
        /// <returns>Скопированный файл</returns>
        Task<IFileStorageItem> Copy(IFileStorageFolder othePath);
        /// <summary>
        /// Загрузка изображения предварительного просмотра
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
