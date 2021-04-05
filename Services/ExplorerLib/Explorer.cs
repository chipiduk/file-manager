namespace ExplorerLib
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Реализует набор стандартных функций для доступа к
    /// файловой системе через диалоговые окна открытия и сохранения файла.
    /// </summary>
    public class Explorer : ExplorerLib.IExplorer
    {
        private string DefaultDocumentsUserDir = @"C:\";

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
        public IExplorerResult FileOpen(string fileFilter,
                                        string lastFilePath,
                                        string myDocumentsUserDir = null,
                                        string defaultExtension = null,
                                        int selectedExtensionIndex = 1)
        {
            if (string.IsNullOrEmpty(myDocumentsUserDir) == true)
                myDocumentsUserDir = DefaultDocumentsUserDir;

            var dlg = new OpenFileDialog();

            dlg.Multiselect = false;

            string dir = lastFilePath;

            try
            {
                if (System.IO.Directory.Exists(lastFilePath) == false)
                    dir = GetDirectoryFromFilePath(lastFilePath);
            }
            catch
            {
            }

            dlg.InitialDirectory = (dir == null ? myDocumentsUserDir : dir);

            dlg.Filter = fileFilter;
            dlg.FilterIndex = selectedExtensionIndex;

            if (string.IsNullOrEmpty(defaultExtension) == false)
            {
                try
                {
                    dlg.DefaultExt = "." + defaultExtension;
                }
                catch {}
            }

            if (dlg.ShowDialog().GetValueOrDefault() == true)
            {
                if (string.IsNullOrEmpty(dlg.FileName) == false)
                    return new ExplorerResult(dlg.FileName, dlg.FilterIndex);
            }

            return null;
        }

        /// <summary>
        /// Метод может использоваться для открытия нескольких файлов
        /// через стандартный диалог открытия файлов в Проводнике Windows.
        /// </summary>
        /// <param name="fileFilter"></param>
        /// <param name="lastFilePath"></param>
        /// <param name="myDocumentsUserDir"></param>
        /// <returns></returns>
        public IExplorerMultiFileResult FileOpenMultipleFiles(string fileFilter,
                                                              string lastFilePath,
                                                              string myDocumentsUserDir = null,
                                                              string defaultExtension = null,
                                                              int selectedExtensionIndex = 1)
        {
            if (string.IsNullOrEmpty(myDocumentsUserDir) == true)
                myDocumentsUserDir = DefaultDocumentsUserDir;

            var dlg = new OpenFileDialog();

            dlg.Multiselect = false;

            string dir = lastFilePath;

            try
            {
                if (System.IO.Directory.Exists(lastFilePath) == false)
                    dir = GetDirectoryFromFilePath(lastFilePath);
            }
            catch
            {
            }

            dlg.InitialDirectory = (dir == null ? myDocumentsUserDir : dir);
            dlg.Multiselect = true;
            dlg.Filter = fileFilter;
            dlg.FilterIndex = selectedExtensionIndex;

            if (string.IsNullOrEmpty(defaultExtension) == false)
            {
                try
                {
                    dlg.DefaultExt = "." + defaultExtension;
                }
                catch { }
            }

            if (dlg.ShowDialog().GetValueOrDefault())
                return new ExplorerMultiFileResult(dlg.FileNames, dlg.FilterIndex);

            return null;
        }

        /// <summary>
        /// Сохраните файл с заданным путем <parameter name = "path" /> (который может быть опущен -> приводит к SaveAs)
        ///
        /// Для <param name = "saveAsFlag" /> можно задать значение true, чтобы указать, предназначена ли функция SaveAs.
        /// <Param name = "FileExtensionFilter" /> можно использовать для фильтрации файлов при использовании диалогового окна "Сохранить как".
        /// 
        /// Функция-оболочка Save Dialog возвращает допустимую строку, если диалог был завершен с
        /// помощью OK, а в противном случае - null (если пользовательская функция отменена).
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stringDiff"></param>
        /// <param name="saveAsFlag"></param>
        /// <param name="FileExtensionFilter"></param>
        /// <returns></returns>
        public IExplorerResult SaveDocumentFile(string path,
                                                string myDocumentsUserDir = null,
                                                bool saveAsFlag = false,
                                                string FileExtensionFilter = "",
                                                string defaultExtension = null,
                                                int selectedExtensionIndex = 1)
        {
            if (string.IsNullOrEmpty(myDocumentsUserDir) == true)
                myDocumentsUserDir = DefaultDocumentsUserDir;

            string filePath = (path == null ? string.Empty : path);

            // Предложить диалоговое окно «Сохранить как файл», если файл никогда ранее не сохранялся (был создан с помощью новой команды)
            //  saveAsFlag = saveAsFlag | !fileToSave.IsFilePathReal;

            try
            {
                if (filePath == string.Empty || saveAsFlag == true)   // Выполнить функцию SaveAs
                {
                    var dlg = new SaveFileDialog();

                    try
                    {
                        dlg.FileName = System.IO.Path.GetFileName(filePath);
                    }
                    catch
                    {
                    }

                    string dir = GetDirectoryFromFilePath(path);
                    dlg.InitialDirectory = (dir == null ? myDocumentsUserDir : dir);

                    if (string.IsNullOrEmpty(FileExtensionFilter) == false)
                    {
                        dlg.Filter = FileExtensionFilter;
                        dlg.FilterIndex = selectedExtensionIndex;

                        if (string.IsNullOrEmpty(defaultExtension) == false)
                        {
                            try
                            {
                                dlg.DefaultExt = "." + defaultExtension;
                            }
                            catch { }
                        }
                    }

                    if (dlg.ShowDialog().GetValueOrDefault() == true)     // Сохранить как файл, если пользователь дал согласие
                    {
                        filePath = dlg.FileName;

                        return new ExplorerResult(dlg.FileName, dlg.FilterIndex);
                    }
                    else
                        return null;
                }                      // Выполнить функцию сохранения

                return null;
            }
            catch (Exception Exp)
            {
                string sMsg = Local.Strings.STR_MSG_ErrorSavingFile;

                if (filePath.Length > 0)
                    sMsg = string.Format(CultureInfo.CurrentCulture, Local.Strings.STR_MSG_ErrorWhileSavingFileX, Exp.Message, filePath);
                else
                    sMsg = string.Format(CultureInfo.CurrentCulture, Local.Strings.STR_MSG_ErrorWhileSavingAFile, Exp.Message);

                throw new Exception(sMsg, Exp);
            }
        }

        /// <summary>
        /// Получите ссылку на имя пути к файлу и верните содержащий путь (если есть)
        /// </summary>
        /// <param name="lastFilePath"></param>
        /// <returns></returns>
        public string GetDirectoryFromFilePath(string lastFilePath)
        {
            string dir = null;

            try
            {
                if (string.IsNullOrEmpty(lastFilePath) == false)
                {
                    dir = System.IO.Path.GetDirectoryName(lastFilePath);

                    if (System.IO.Directory.Exists(dir) == false)
                        dir = null;
                }
            }
            catch
            {
            }

            return dir;
        }
    }
}
