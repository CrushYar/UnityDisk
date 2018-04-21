using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Settings
{
    /// <summary>
    /// Базовый класс настроек
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Получение потока данных из настроек по указанному параметру
        /// </summary>
        /// <param name="parameterName">Название параметра</param>
        /// <param name="encoding">Кодировка байтов</param>
        /// <returns>Поток данных ассоциированный из указанным параметром</returns>
        Stream GetValueAsStream(string parameterName, Encoding encoding = null);

        /// <summary>
        /// Получение строку данных из настроек по указанному параметру
        /// </summary>
        /// <param name="parameterName">Название параметра</param>
        /// <returns>строка данных ассоциированный из указанным параметром</returns>
        string GetValueAsString(string parameterName);
        /// <summary>
        /// Устанавливает поток данных ассоциировая с указанным параметром
        /// </summary>
        /// <param name="parameterName">Параметр с которым будет ассоциирован поток</param>
        /// <param name="value">Поток для записи</param>
        /// <param name="encoding">Кодировка байтов</param>
        void SetValueAsStream(string parameterName, Stream value, Encoding encoding = null);
        /// <summary>
        /// Устанавливает строку данных ассоциировая с указанным параметром
        /// </summary>
        /// <param name="parameterName">Параметр с которым будет ассоциированна строка данных</param>
        /// <param name="value">Поток для записи</param>
        void SetValueAsString(string parameterName, string value);
    }
}
