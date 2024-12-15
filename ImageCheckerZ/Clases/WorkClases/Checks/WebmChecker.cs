using ImageCheckerZ.Clases.DataClases;
using ImageCheckerZ.Clases.WorkClases.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageCheckerZ.Clases.DataClases.Global.Checks.Enums;

namespace ImageCheckerZ.Clases.WorkClases.Checks
{
    /// <summary>
    /// Класс выполнения проверки для Webm
    /// </summary>
    internal class WebmChecker : CheckFileBase
    {
        //О формате: тут жопа


        /// <summary>
        /// Структура, описывающая формат
        /// </summary>
        public override FormatInfo Info => new FormatInfo() { 
            Name = "Webm",
            Type = FileTypes.Webm,
            Extensions = new List<string>() {
                ".webm"
            },
            MimeTypes = new List<string>() {
                "video/webm", "audio/webm"
            },
        };


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public WebmChecker()
        {

        }



        /// <summary>
        /// Проверка на то, что формат файла является текущим
        /// </summary>
        /// <param name="path">Строка пути к файлу</param>
        /// <returns>True - формат файла соответствует проверке</returns>
        public override bool IsCurrentFormat(string path) =>
            //Тут просто чекаем расширение
            IsCurrentExt(path);


        /// <summary>
        /// Проверка корректности считанного файла
        /// </summary>
        /// <param name="path">Строка пути к файлу</param>
        /// <returns>True - файл корректен</returns>
        public override bool CheckFile(string path)
        {
            //Получаем байты файла
            byte[] bytes = LoadBytes(path);
            //Выполняем проверки подряд: по наличию контента
            return IsFileNotEmpty(bytes);
        }
    }
}
