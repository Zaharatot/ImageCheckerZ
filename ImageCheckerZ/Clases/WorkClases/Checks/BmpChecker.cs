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
    /// Класс выполнения проверки для Bmp
    /// </summary>
    internal class BmpChecker : CheckFileBase
    {
        //О формате: 
        //https://jenyay.net/Programming/Bmp


        /// <summary>
        /// Байты заголовка файла Bmp
        /// </summary>
        private readonly byte[] _startBmp = { 0x42, 0x4D };



        /// <summary>
        /// Структура, описывающая формат
        /// </summary>
        public override FormatInfo Info => new FormatInfo() { 
            Name = "Bmp",
            Type = FileTypes.Bmp,
            Extensions = new List<string>() { 
                ".bmp", ".dib"
            },
            MimeTypes = new List<string>() {
                "image/bmp", "image/x-bmp"
            },
        };


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public BmpChecker()
        {

        }

        /// <summary>
        /// Метод проверки наличия байт начала файла
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - заголовок корректен</returns>
        private bool IsContainHeader(byte[] bytes) =>
            //Сверяем первые 2 байта файла с эталонными
            bytes.Take(2).SequenceEqual(_startBmp);

        /// <summary>
        /// Метод проверки корректности размера файла
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - размер корректен</returns>
        private bool IsFileSizeCorrect(byte[] bytes)
        {
            //Получаем размер файла
            byte[] bfSize = bytes.Skip(2).Take(4).ToArray();
            //Размер файла в байтах
            int length = BitConverter.ToInt32(bfSize, 0);
            //Чекаем корректность итогового размера файла с размером из заголовка
            return (bytes.Length == length);
        }







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
                //По корректности размера
                && IsFileSizeCorrect(bytes);
        }
    }
}
