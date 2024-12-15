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
    /// Класс выполнения проверки для Webp
    /// </summary>
    internal class WebpChecker : CheckFileBase
    {
        //О формате: 
        //https://developers.google.com/speed/webp/docs/riff_container?hl=ru



        /// <summary>
        /// Байты заголовка RIFF
        /// </summary>
        private readonly byte[] _startRiff = { 0x52, 0x49, 0x46, 0x46, };
        /// <summary>
        /// Байты заголовка WEBP
        /// </summary>
        private readonly byte[] _startWebp = { 0x57, 0x45, 0x42, 0x50, };


        /// <summary>
        /// Структура, описывающая формат
        /// </summary>
        public override FormatInfo Info => new FormatInfo() { 
            Name = "Webp",
            Type = FileTypes.Webp,
            Extensions = new List<string>() {
                ".webp"
            },
            MimeTypes = new List<string>() {
                "image/webp"
            },
        };


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public WebpChecker()
        {

        }

        /// <summary>
        /// Метод проверки наличия байт заголовка RIFF
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - заголовок корректен</returns>
        private bool IsContainRiffHeader(byte[] bytes) =>
            //Сверяем первые 4 байта файла с эталонными
            bytes.Take(4).SequenceEqual(_startRiff);

        /// <summary>
        /// Метод проверки наличия байт заголовка WEBP
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - заголовок корректен</returns>
        private bool IsContainWebpHeader(byte[] bytes) =>
            //Сверяем 4 байта после первых заголовков файла с эталонными
            bytes.Skip(8).Take(4).SequenceEqual(_startWebp);


        /// <summary>
        /// Метод проверки корректности размера файла
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - размер корректен</returns>
        private bool IsFileSizeCorrect(byte[] bytes)
        {
            //Получаем размер файла
            byte[] bfSize = bytes.Skip(4).Take(4).ToArray();
            //Размер файла в байтах, плюс размеры заголовков
            int length = BitConverter.ToInt32(bfSize, 0) + 8;
            //Чекаем корректность итогового размера файла с размером из заголовка
            return (bytes.Length == length);
        }







        /// <summary>
        /// Проверка на то, что формат файла является текущим
        /// </summary>
        /// <param name="path">Строка пути к файлу</param>
        /// <returns>True - формат файла соответствует проверке</returns>
        public override bool IsCurrentFormat(string path)
        {
            //Получаем байты файла
            byte[] bytes = LoadBytes(path);
            //Проверяем наличие заголовков файла
            return (IsContainRiffHeader(bytes) && IsContainWebpHeader(bytes)) ||
            //Если заголовок не совпал - чекаем расширение
            IsCurrentExt(path);
        }


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
                //По наличию заголовка RIFF
                && IsContainRiffHeader(bytes)
                //По наличию заголовка WEBP
                && IsContainWebpHeader(bytes)
                //По корректности размера
                && IsFileSizeCorrect(bytes);
        }
    }
}
