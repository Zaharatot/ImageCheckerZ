using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCheckerZ.Clases.DataClases.Global.Checks
{
    /// <summary>
    /// Класс глобальных перечислений
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Внутренние типы файлов
        /// </summary>
        public enum FileTypes
        {
            /// <summary>
            /// Формат файла PNG
            /// </summary>
            Png = 0,
            /// <summary>
            /// Формат файла Jpeg
            /// </summary>
            Jpeg = 1,
            /// <summary>
            /// Формат файла Gif
            /// </summary>
            Gif = 2,
            /// <summary>
            /// Формат файла Bmp
            /// </summary>
            Bmp = 3,
            /// <summary>
            /// Формат файла Webp
            /// </summary>
            Webp = 4,
            /// <summary>
            /// Формат файла Webm
            /// </summary>
            Webm = 5,
        }

    }
}
