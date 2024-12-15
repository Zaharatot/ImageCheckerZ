using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCheckerZ.Clases.DataClases.Global
{
    /// <summary>
    /// Интерфейс, описывающий класс проверки
    /// </summary>
    internal interface IFileCheck
    {

        /// <summary>
        /// Структура, описывающая формат
        /// </summary>
        FormatInfo Info { get; }




        /// <summary>
        /// Проверка на то, что формат файла является текущим
        /// </summary>
        /// <param name="path">Строка пути к файлу</param>
        /// <returns>True - формат файла соответствует проверке</returns>
        bool IsCurrentFormat(string path);

        /// <summary>
        /// Проверка корректности считанного файла
        /// </summary>
        /// <param name="path">Строка пути к файлу</param>
        /// <returns>True - файл корректен</returns>
        bool CheckFile(string path);
    }
}
