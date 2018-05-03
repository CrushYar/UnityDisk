using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts.Registry;

namespace UnityDisk.StorageItems
{
    /// <summary>
    /// Интерфейс для связывания элемента файловой системы UnityDisk с элементом ФС пердставления конкретного сервиса 
    /// </summary>
    public interface IStorageItem2
    {
        /// <summary>
        /// Публичная ссылка
        /// </summary>
        string PublicUrl { get; }
        /// <summary>
        /// Аккаунт
        /// </summary>
        IAccountProjection Account { get; set; }
        /// <summary>
        /// Дата создания элемента
        /// </summary>
        DateTime CreateDate { get; }
    }
}
