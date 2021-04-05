namespace ExplorerLib
{
    using System.Collections.Generic;

    internal class ExplorerMultiFileResult : IExplorerMultiFileResult
    {
        private List<string> _FilePath = null;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="filepaths"></param>
        /// <param name="selectedFilterIndex"></param>
        public ExplorerMultiFileResult(IEnumerable<string> filepaths
                                     , int selectedFilterIndex)
            : this()
        {
            if (filepaths != null)
            {
                foreach (var item in filepaths)
                    _FilePath.Add(item);
            }

            SelectedFilterIndex = selectedFilterIndex;
        }

        /// <summary>
        /// Скрытый безпараметрический ctor
        /// </summary>
        protected ExplorerMultiFileResult()
        {
            _FilePath = new List<string>();
        }

        /// <summary>
        /// Получает полный путь к выбранному файлу.
        /// </summary>
        public IEnumerable<string> Filepaths
        {
            get
            {
                return _FilePath;
            }
        }

        /// <summary>
        /// Получает информацию об индексе фильтра для выбранного фильтра файла.
        /// </summary>
        public int SelectedFilterIndex { get; private set; }
    }
}
