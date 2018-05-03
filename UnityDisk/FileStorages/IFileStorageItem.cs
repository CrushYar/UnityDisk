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
        /// Атрибут элемента
        /// </summary>
        StorageItemAttributeEnum Attribute { get; }
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
        void Delete();
        /// <summary>
        /// Переименование элемента
        /// </summary>
        /// <param name="newName">Новое имя</param>
        void Rename(string newName);
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="folder">Папка в которую нужно переместить элемент</param>
        void Move(IFileStorageFolder folder);
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="othePath">Целевой путь</param>
        void Copy(IFileStorageFolder othePath);
        /// <summary>
        /// Загрузка изображения предварительного просмотра
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
        /// <summary>
        /// Импортирует данные из строки
        /// </summary>
        /// <param name="data">Данные в строковом виде</param>
        void Parse(String data);
        /// <summary>
        /// Получение данных в строковом виде
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}
