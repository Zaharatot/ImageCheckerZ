# ImageCheckerZ

//Readme RU
Данная библиотека предназначена для проверки корректности файлов основных форматов изображений, которые распространены в интернете.
Работать с библиотекой необходимо через класс "ImageCheckerFasade", в нём присутствуют два метода:
1. Метод "GetFormatInfo" возвращает описание формата файла по пути к файлу. Проверка выполняется сначала по заголовкам, затем - по рассширению (на случай, если файл был переименован или пуст).
2. Метод "CheckFile" выполняет проверку содержимого файла, по пути к нему и типу, который можно получить из метода "GetFormatInfo".
В проекте "TestImageCheck" продемонстировано взаимодействие с данной библиотекой.

На данный момент, библиотекой полноценно поддерживается выполнение проверки для файлов форматов Jpg, Png, Bmp, Gif, Webp. 
Есть условная поддержка Webm, но на уровне определения по расширению и проверки на то что файл пуст - очень уж там большая вариативность контента.


//Readme EN
This library is designed to check the correctness of the files of the main image formats that are distributed on the Internet.
It is necessary to work with the library through the "ImageCheckerFasade" class, it contains two methods:
1. The "GetFormatInfo" method returns a description of the file format along the path to the file. The check is performed first by headers, then by extension (in case the file has been renamed or is empty).
2. The "CheckFile" method checks the contents of the file, along the path to it and the type that can be obtained from the "GetFormatInfo" method.
The "TestImageCheck" project demonstrates interaction with this library.

At the moment, the library fully supports checking for Jpg, Png, Bmp, Gif, Webp files. 
There is conditional support for Webm, but at the level of determining by extension and checking that the file is empty, there is too much variability in content.
