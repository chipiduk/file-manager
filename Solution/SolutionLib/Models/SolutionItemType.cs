namespace SolutionLib.Models
{
    /// <summary>
    /// Определяет типизированный идентификатор элемента в решении.
    /// </summary>
    public enum SolutionItemType
    {
        /// <summary>
        /// Представляет корень элементов в дереве решения.
        /// </summary>
        SolutionRootItem = 0,

        /// <summary>
        /// Общий файл.
        /// </summary>
        File = 100,

        /// <summary>
        /// Папка общего решения.
        /// </summary>
        Folder = 200,

        /// <summary>
        /// Общий проект.
        /// </summary>
        Project = 300
    }
}
