namespace ExplorerLib
{
    /// <summary>
    /// Реализует простую структуру результата,
    /// которая возвращается, когда пользователь
    /// выбирает файл в диалоговом окне.
    /// </summary>
    public interface IExplorerResult
    {
        /// <summary>
        /// Получает полный путь к выбранному файлу.
        /// </summary>
        string Filepath { get; }

        /// <summary>
        /// Получает расширение выбранного файла.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Получает информацию о пути к каталогу выбранного файла.
        /// </summary>
        string FileDirectory { get; }

        /// <summary>
        /// Получает информацию об индексе фильтра для выбранного фильтра файла.
        /// </summary>
        int SelectedFilterIndex { get; }
    }
}
