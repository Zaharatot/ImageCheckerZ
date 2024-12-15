using ImageCheckerZ.Clases.DataClases;
using ImageCheckerZ.Clases.DataClases.Global;
using ImageCheckerZ.Clases.WorkClases.Checks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageCheckerZ.Clases.DataClases.Global.Checks.Enums;

namespace ImageCheckerZ
{
    /// <summary>
    /// Фасадный класс библиотеки выполнения проверок
    /// </summary>
    public class ImageCheckerFasade
    {
        /// <summary>
        /// Список проверок для файлов
        /// </summary>
        private List<IFileCheck> _checks;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ImageCheckerFasade()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем список проверок
            _checks = CreateChecks();
        }

        /// <summary>
        /// Метод инициализации списка проверок
        /// </summary>
        /// <returns>Список классов проверки</returns>
        private List<IFileCheck> CreateChecks() =>
            new List<IFileCheck>() { 
                new BmpChecker(),
                new GifChecker(),
                new JpegChecker(),
                new PngChecker(),
                new WebmChecker(),
                new WebpChecker()
            };



        /// <summary>
        /// Метод получения информации о формате по пути к файлу
        /// </summary>
        /// <param name="path">Строка пути к файлу</param>
        /// <returns>Информация о формате</returns>
        public FormatInfo? GetFormatInfo(string path)
        {
            //Если файл существует
            if (File.Exists(path))
            {
                //Проходимся по проверкам
                foreach (IFileCheck check in _checks)
                    //Если файл соответствует проверке
                    if (check.IsCurrentFormat(path))
                        //Возвращаем информацию о нём
                        return check.Info;
            }
            //По дефолту вернём null
            return null;
        }


        /// <summary>
        /// Метод выполнения проверки корректности файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="type">Тип файла</param>
        /// <returns>True - файл корректен</returns>
        public bool CheckFile(string path, FileTypes type)
        {
            //Получаем проверку по типу
            IFileCheck check = _checks.FirstOrDefault(ch => ch.Info.Type == type);
            //Выполняем проверку по типу
            return check.CheckFile(path);
        }
    }
}
