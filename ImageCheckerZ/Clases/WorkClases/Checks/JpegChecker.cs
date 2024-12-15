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
    /// Класс выполнения проверки для JPEG
    /// </summary>
    internal class JpegChecker : CheckFileBase
    {
        //О формате: 
        //https://dzen.ru/a/Za9SSG5yRhxBsiXx


        /// <summary>
        /// Байты заголовка файла JPEG
        /// </summary>
        private readonly byte[] _startJpeg = { 0xFF, 0xD8 };
        /// <summary>
        /// Байты завершения файла JPEG
        /// </summary>
        private readonly byte[] _endJpeg = { 0xFF, 0xD9 };

        /// <summary>
        /// Структура, описывающая формат
        /// </summary>
        public override FormatInfo Info => new FormatInfo() { 
            Name = "Jpeg",
            Type = FileTypes.Jpeg,
            Extensions = new List<string>() { 
                ".jpeg", ".jpg", ".jpe", ".jfif", ".jif"
            },
            MimeTypes = new List<string>() {
                "image/jpeg"
            },
        };


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public JpegChecker()
        {

        }

        /// <summary>
        /// Метод проверки наличия байт начала файла
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - заголовок корректен</returns>
        private bool IsContainHeader(byte[] bytes) =>
            //Сверяем первые 2 байта файла с эталонными
            bytes.Take(2).SequenceEqual(_startJpeg);

        /// <summary>
        /// Метод проверки наличия байт конца файла
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - конец корректен</returns>
        private bool IsContainFooter(byte[] bytes) =>
            //Ищем в файле байты конца - они могут быть не в самом конце файла!
            bytes.Intersect(_endJpeg).Any();




        /// <summary>
        /// Проверка на то, что формат файла является текущим
        /// </summary>
        /// <param name="path">Строка пути к файлу</param>
        /// <returns>True - формат файла соответствует проверке</returns>
        public override bool IsCurrentFormat(string path) =>
            //Проверяем наличие заголовка файла
            IsContainHeader(LoadBytes(path)) || 
            //Если заголовок не совпал - чекаем расширение
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
            return IsFileNotEmpty(bytes) 
                //По наличию заголовка
                && IsContainHeader(bytes) 
                //По наличию окончания
                && IsContainFooter(bytes);
        }
    }
}
