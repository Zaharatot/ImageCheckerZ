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
    /// Класс выполнения проверки для Gif
    /// </summary>
    internal class GifChecker : CheckFileBase
    {
        //О формате: 
        //https://giflib.sourceforge.net/whatsinagif/bits_and_bytes.html


        /// <summary>
        /// Байты заголовка файла Bmp
        /// </summary>
        private readonly byte[] _startGif = { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };
        /// <summary>
        /// Байт конца файла
        /// </summary>
        private readonly byte _endGif = 0x3B;



        /// <summary>
        /// Структура, описывающая формат
        /// </summary>
        public override FormatInfo Info => new FormatInfo() { 
            Name = "Gif",
            Type = FileTypes.Gif,
            Extensions = new List<string>() {
                ".gif"
            },
            MimeTypes = new List<string>() {
                "image/gif"
            },
        };


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public GifChecker()
        {

        }

        /// <summary>
        /// Метод проверки наличия байт начала файла
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - заголовок корректен</returns>
        private bool IsContainHeader(byte[] bytes) =>
            //Сверяем первые байты файла с эталонными
            bytes.Take(_startGif.Length).SequenceEqual(_startGif);

        /// <summary>
        /// Метод проверки наличия байт конца файла
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - конец корректен</returns>
        private bool IsContainFooter(byte[] bytes) =>
            //Сверяем последний байт файла с эталонным
            bytes.Last().Equals(_endGif);







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
                //По корректности последнего байта
                && IsContainFooter(bytes);
        }
    }
}
