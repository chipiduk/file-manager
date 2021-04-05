namespace SolutionLib.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Реализует базовый класс для определенного типа элемента коллекции, который может 
    /// иметь дочерние элементы, и предоставляет функции для управления коллекцией дочерних
    /// элементов (удаление, переименование, добавление дочернего элемента и т.д.)
    /// </summary>
    public interface IItemChildren : IItem
    {
        /// <summary>
        /// Получает все дочерние элементы этого (родительского) элемента.
        /// </summary>
        IEnumerable<IItem> Children { get; }

        #region methods
        /// <summary>
        /// Находит дочерний элемент по заданному ключу или возвращает null.
        /// </summary>
        /// <param name="displyName"></param>
        /// <returns></returns>
        IItem FindChild(string displyName);

        /// <summary>
        /// Добавление нового следующего дочернего элемента через Inplace Edit Box
        /// требует, чтобы мы знали, является ли «Новая папка», «Новая папка 1», 
        /// «Новая папка 2» ... следующим подходящим именем - этот метод определяет
        /// это имя и возвращает его для заданный тип (создаваемого) дочернего элемента.
        /// </summary>
        /// <param name="nextTypeTpAdd"></param>
        /// <returns></returns>
        string SuggestNextChildName(Models.SolutionItemType nextTypeTpAdd);

        /// <summary>
        /// Добавляет дочерний элемент типа <see cref = "IItem" /> к этому
        /// родительскому элементу, который также можно ввести с помощью <see cref = "IItem" />.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IItem AddChild(IItem item);

        /// <summary>
        /// Добавляет дочерний элемент с заданным типом 
        /// (здесь нельзя добавить <see cref = "SolutionLib.Models.SolutionItemType" />).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IItem AddChild(string displayName, Models.SolutionItemType type);

        /// <summary>
        /// Удаляет дочерний элемент из коллекции дочерних элементов этого элемента.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool RemoveChild(IItem item);

        /// <summary>
        /// Переименовывает дочерний элемент в коллекцию дочерних элементов этого элемента.
        /// После переименования следует применить повторную сортировку и IsItemSelected,
        /// чтобы переименованный объект снова появился в правильной позиции в отсортированном списке элементов.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        void RenameChild(IItem item, string newName);

        /// <summary>
        /// Удаляет всех дочерних элементов (если есть) ниже этого элемента.
        /// </summary>
        void RemoveAllChild();

        /// <summary>
        /// Сортирует все элементы для отображения в отсортированном виде.
        /// </summary>
        void SortChildren();

        /// <summary>
        /// Добавляет другой элемент папки (дочерний) в данную коллекцию элементов.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IItem AddFolder(string displayName);

        /// <summary>
        /// Добавляет другой проект (дочерний) элемент в данную коллекцию элементов.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IItem AddProject(string displayName);

        /// <summary>
        /// Добавляет другой файл (дочерний) элемент в данную коллекцию элементов.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IItem AddFile(string displayName);
        #endregion methods
    }
}
