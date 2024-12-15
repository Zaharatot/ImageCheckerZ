using ImageCheckerZ.Clases.DataClases;
using ImageCheckerZ.Clases.DataClases.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCheckerZ.Clases.WorkClases.Base
{
    /// <summary>
    /// Базовый класс выполнения проверки файла
    /// </summary>
    internal class CheckFileBase : IFileCheck
    {
        /// <summary>
        /// Структура, описывающая формат
        /// </summary>
        public virtual FormatInfo Info => new FormatInfo();



        /// <summary>
        /// Метод выполнения загрузки байт файла
        /// </summary>
        /// <param name="path">Строка пути к файлу</param>
        internal byte[] LoadBytes(string path) =>
            File.ReadAllBytes(path);

        /// <summary>
        /// Метод проверки на наличие байт в файле
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - файл корректен</returns>
        internal bool IsFileNotEmpty(byte[] bytes) =>
            //Проверяем, что файл не заполнен нолями
            !bytes.All(bt => bt == 0);

        /// <summary>
        /// Метод проверки расширения файла
        /// </summary>
        /// <param name="path">Строка пути к файлу</param>
        /// <returns>True - расширение совпадает со списочным</returns>
        internal bool IsCurrentExt(string path) =>
            //Получаем расширение из пути, и сверяем с расширением из описания
            Info.Extensions.Contains(Path.GetExtension(path).ToLower());



        /// <summary>
        /// Проверка на то, что формат файла является текущим
        /// </summary>
        /// <param name="path">Строка пути к файлу</param>
        /// <returns>True - формат файла соответствует проверке</returns>
        public virtual bool IsCurrentFormat(string path) => false;

        /// <summary>
        /// Проверка корректности считанного файла
        /// </summary>
        /// <param name="path">Строка пути к файлу</param>
        /// <returns>True - файл корректен</returns>
        public virtual bool CheckFile(string path) => false;
    }
}
