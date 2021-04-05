namespace ExplorerLib
{
    using System.Collections.Generic;

    /// <summary>
    /// Реализует интерфейс для объекта, который реализует
    /// набор стандартных функций для доступа к файловой системе
    /// через диалоговые окна открытия и сохранения файла.
    /// </summary>
    public interface IExplorer
    {
        /// <summary>
        ///    Позвольте пользователю выбрать файл для
        ///    открытия -> вернуть его путь, если файл
        ///    был открыт, или вернуть null при отмене.
        /// </summary>
        /// <param name="fileFilter"></param>
        /// <param name="lastFilePath"></param>
        /// <param name="myDocumentsUserDir"></param>
        /// <param name="defaultExtension"></param>
        /// <param name="selectedExtensionIndex"></param>
        /// <returns></returns>
        IExplorerResult FileOpen(string fileFilter,
                                string lastFilePath,
                                string myDocumentsUserDir = null,
                                string defaultExtension = null,
                                int selectedExtensionIndex = 1);

        /// <summary>
        /// Метод может использоваться для открытия нескольких
        /// файлов через стандартный диалог открытия файлов в Проводнике Windows.
        /// </summary>
        /// <param name="fileFilter"></param>
        /// <param name="lastFilePath"></param>
        /// <param name="myDocumentsUserDir"></param>
        /// <param name="defaultExtension"></param>
        /// <param name="selectedExtensionIndex"></param>
        /// <returns></returns>
        IExplorerMultiFileResult FileOpenMultipleFiles(string fileFilter,
                                                       string lastFilePath,
                                                       string myDocumentsUserDir = null,
                                                       string defaultExtension = null,
                                                       int selectedExtensionIndex = 1);

        /// <summary>
        /// Сохраните файл с заданным путем <paramref name = "path" /> (который может быть опущен -> приводит к SaveAs),
        /// используя заданную функцию сохранения <paramref name = "saveDocumentFunction" />, которая принимает строковый
        /// параметр и возвращает bool при успех. Для <param name = "saveAsFlag" /> можно установить значение true, чтобы
        /// указать, предназначена ли функция SaveAs. <Param name = "FileExtensionFilter" /> можно использовать для фильтрации
        /// файлов при использовании диалогового окна "Сохранить как".
        /// </summary>
        /// <param name="path"></param>
        /// <param name="saveDocumentFunction"></param>
        /// <param name="stringDiff"></param>
        /// <param name="saveAsFlag"></param>
        /// <param name="FileExtensionFilter"></param>
        /// <param name="lastFilePath"></param>
        /// <returns></returns>
        string GetDirectoryFromFilePath(string lastFilePath);

        /// <summary>
        /// Сохраните файл с заданным путем <parameter name = "path" /> (который может быть опущен -> приводит к SaveAs)
        ///
        /// Для <param name = "saveAsFlag" /> можно задать значение true, чтобы указать, предназначена ли функция SaveAs.
        /// <Param name = "FileExtensionFilter" /> можно использовать для фильтрации файлов при использовании диалогового окна "Сохранить как".
        /// 
        /// Функция-оболочка Save Dialog возвращает допустимую строку, если диалог был завершен
        /// с помощью OK, а в противном случае - null (если пользовательская функция отменена).
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stringDiff"></param>
        /// <param name="saveAsFlag"></param>
        /// <param name="FileExtensionFilter"></param>
        /// <returns></returns>
        IExplorerResult SaveDocumentFile(string path,
                                         string myDocumentsUserDir = null,
                                         bool saveAsFlag = false,
                                         string FileExtensionFilter = "",
                                         string defaultExtension = null,
                                         int selectedExtensionIndex = 1);
    }
}
