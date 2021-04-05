using System.Collections.Generic;

namespace ExplorerLib
{
    /// <summary>
    /// Реализует простую структуру результатов, которая возвращается,
    /// когда пользователь выбирает несколько файлов в диалоговом окне.
    /// </summary>
    public interface IExplorerMultiFileResult
    {
        /// <summary>
        /// Получает полный путь к выбранному файлу.
        /// </summary>
        IEnumerable<string> Filepaths { get; }

        /// <summary>
        /// Получает информацию об индексе фильтра для выбранного фильтра файла.
        /// </summary>
        int SelectedFilterIndex { get; }
    }
}
