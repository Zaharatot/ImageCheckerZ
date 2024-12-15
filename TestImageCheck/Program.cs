using ImageCheckerZ;
using ImageCheckerZ.Clases.DataClases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestImageCheck
{
    internal class Program
    {


        static void Main(string[] args)
        {

            FormatInfo? info;
            //Инициализируем класс проверок
            ImageCheckerFasade imageChecker = new ImageCheckerFasade();
            //Путь к тестовой папке
            string testDirPath = Environment.CurrentDirectory + "\\Tests\\";
            //На всякий случай - создаём её
            Directory.CreateDirectory(testDirPath);
            //массив тестовых строк путей
            string[] paths = Directory.GetFiles(testDirPath);
            //Проходимся по путям
            foreach (string path in paths)
            {
                //Выводим заголовок
                Console.WriteLine($"Файл: {path}");

                //Получаем информацию о форммате файла
                info = imageChecker.GetFormatInfo(path);
                //Если инфа есть
                if (info.HasValue)
                {
                    //Выводим инфу о файле
                    Console.WriteLine(info.Value.ToString());
                    //Чекаем корректность файла
                    bool isCorrect = imageChecker.CheckFile(path, info.Value.Type);
                    //Выводим результат
                    Console.WriteLine($"\tРезультат проверки файла: {isCorrect}");
                }
                //Если инфы нет
                else
                    //Выводим ошибку
                    Console.WriteLine($"\tОшибка: Файл отсутствует, или его тип не известен!");
                //Отчерчивание перед следующим файлом
                Console.WriteLine("[----------------------------------------------]");
            }
            Console.ReadLine();
        }







    }
}
