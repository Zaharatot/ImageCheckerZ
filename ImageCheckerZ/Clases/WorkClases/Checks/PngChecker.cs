using ImageCheckerZ.Clases.DataClases;
using ImageCheckerZ.Clases.WorkClases.Base;
using ImageCheckerZ.Clases.WorkClases.Checksumm;
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
    /// Класс выполнения проверки для Png
    /// </summary>
    internal class PngChecker : CheckFileBase
    {
        //О формате: 
        //https://dzen.ru/a/Za9SSG5yRhxBsiXx



        /// <summary>
        /// Длинна подписи чанка
        /// </summary>
        const int PNG_SIGN_LENGTH = 8;
        /// <summary>
        /// Длинна подписи чанка
        /// </summary>
        const int CHUNK_SIGN_LENGTH = 4;
        /// <summary>
        /// Длинна размера чанка
        /// </summary>
        const int CHUNK_SIZE_LENGTH = 4;
        /// <summary>
        /// Длинна типа чанка
        /// </summary>
        const int CHUNK_TYPE_LENGTH = 4;


        /// <summary>
        /// Байты заголовка файла PNG
        /// </summary>
        private readonly byte[] _sign = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        /// <summary>
        /// Байты завершающего чанка PNG
        /// </summary>
        private readonly byte[] _endChunk = { 0x0, 0x0, 0x0, 0x0, 0x49, 0x45, 0x4E, 0x44, 0xAE, 0x42, 0x60, 0x82 };


        /// <summary>
        /// Структура, описывающая формат
        /// </summary>
        public override FormatInfo Info => new FormatInfo() { 
            Name = "Png",
            Type = FileTypes.Png,
            Extensions = new List<string>() {
                ".png"
            },
            MimeTypes = new List<string>() {
                "image/png"
            },
        };

        /// <summary>
        /// Класс рассчёта контрольной суммы по алгоритму CRC-32
        /// </summary>
        private Crc32 _crc32;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public PngChecker()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Получаем экземпляр класса рассчёта контрольной суммы
            _crc32 = Crc32.GetInstance();
        }



        /// <summary>
        /// Метод проверки наличия байт начала файла
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - заголовок корректен</returns>
        private bool IsContainHeader(byte[] bytes) =>
            //Сверяем первые байты файла с эталонными
            bytes.Take(_sign.Length).SequenceEqual(_sign);

        /// <summary>
        /// Метод проверки наличия байт конца файла
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - конец корректен</returns>
        private bool IsContainFooter(byte[] bytes) =>
            //Сверяем последние байты файла с эталонными
            bytes.Skip(bytes.Length - _endChunk.Length).SequenceEqual(_endChunk);


        /// <summary>
        /// Чекаем чанк пикселей
        /// </summary>
        /// <param name="bytes">Байты чанка</param>
        /// <returns>True - чанк корректен</returns>
        private bool CheckChunk(ref byte[] bytes)
        {
            //Получаем байты размера чанка
            byte[] chunkSizeBytes = bytes.Take(CHUNK_SIZE_LENGTH).ToArray();
            //Получаем размер чанка
            int chunkDataSize = BitConverter.ToInt32(chunkSizeBytes.Reverse().ToArray(), 0) + CHUNK_TYPE_LENGTH;
            //Получаем байты контента чанка
            byte[] chunkData = bytes.Skip(CHUNK_SIZE_LENGTH).Take(chunkDataSize).ToArray();
            //Получаем контрольную сумму чанка
            byte[] chunkChecksumm = bytes.Skip(CHUNK_SIZE_LENGTH + chunkDataSize).Take(CHUNK_SIGN_LENGTH).ToArray();
            //Считаем контрольную сумму чанка
            byte[] chunkCalculatedChecksumm = _crc32.ComputeChecksum(chunkData);
            //Сбрасываем проверенный чанк
            bytes = bytes.Skip(chunkDataSize + CHUNK_SIZE_LENGTH + CHUNK_SIGN_LENGTH).ToArray();
            //Чекаем корректность контрольной суммы чанка
            return chunkChecksumm.SequenceEqual(chunkCalculatedChecksumm);
        }

        /// <summary>
        /// Метод проверки каждого из чанков PNG
        /// </summary>
        /// <param name="bytes">Байты файла для проверки</param>
        /// <returns>True - чанки корректны</returns>
        private bool CheckChunks(byte[] bytes)
        {
            //Получаем байты от начала чанков
            byte[] imageData = bytes.Skip(PNG_SIGN_LENGTH).ToArray();
            //Флаг по дефолту - корректный
            bool isChuncsCorrect = true;
            //Цикл идёт, пока есть чанки и они корректны
            while (isChuncsCorrect && imageData.Length > 0)
                //Чекаем чанк
                isChuncsCorrect = CheckChunk(ref imageData);
            //Возвращаем флаг корректности чанка
            return isChuncsCorrect;
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
                //Чанки корректны
                && CheckChunks(bytes)
                //По наличию окончания
                && IsContainFooter(bytes);
        }
    }
}
