using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageCheckerZ.Clases.DataClases.Global.Checks.Enums;

namespace ImageCheckerZ.Clases.DataClases
{
    /// <summary>
    /// Структура, описывающая формат файла
    /// </summary>
    public struct FormatInfo
    {
        /// <summary>
        /// Название формата
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Список расширенйи для формата
        /// </summary>
        public List<string> Extensions { get; set; }
        /// <summary>
        /// Список маймтайпов формата
        /// </summary>
        public List<string> MimeTypes { get; set; }
        /// <summary>
        /// Внутренний тип формата
        /// </summary>
        public FileTypes Type { get; set; }


        /// <summary>
        /// Метод конвертации в строку
        /// </summary>
        /// <returns>Строка результата</returns>
        public override string ToString()
        {
            //Инициализируем билдер строк
            StringBuilder sb = new StringBuilder();
            //Добавляем единичные значения
            sb.AppendLine($"[Name: {Name}]");
            sb.AppendLine($"[Type: {Type}]");
            //Добавляем массив расширений
            sb.Append("[Extensions:");
            Extensions.ForEach(ext => sb.Append($" {ext}"));
            sb.AppendLine("]");
            //Добавляем массив маймтайпов
            sb.Append("[MimeTypes:");
            MimeTypes.ForEach(mt => sb.Append($" {mt}"));
            sb.AppendLine("]");
            //Возвращаем итоговую строку
            return sb.ToString(); 
        }
    }
}
