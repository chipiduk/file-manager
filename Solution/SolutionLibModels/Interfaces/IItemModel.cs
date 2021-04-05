namespace SolutionModelsLib.Interfaces
{
    using SolutionModelsLib.Enums;
    using System.Xml.Serialization;

    /// <summary>
    /// Реализует интерфейс для абстрактного класса, который реализует
    /// элементы, которые НЕ МОГУТ иметь собственных дочерних элементов.
    /// Эти элементы обычно имеют только имя и идентификатор (например, файл),
    /// но не имеют своих собственных коллекций.
    /// </summary>
    public interface IItemModel : IModelBase, IXmlSerializable
    {
        #region properties
        /// <summary>
        /// Получает / задает ID для этого элемента.
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// Получает / устанавливает родительский элемент этого элемента.
        /// </summary>
        IItemModel Parent { get; set; }

        /// <summary>
        /// Получает / устанавливает технический тип этого элемента для ID элемента с точки зрения его возможностей, связанных с этим типом элемента.
        /// </summary>
        SolutionModelItemType ItemType { get; set; }

        /// <summary>
        /// Получает / задает имя для отображения в UI.
        /// </summary>
        string DisplayName { get; set; }

        ////        /// <summary>
        ////        /// Gets/sets whether the <see cref="DisplayName"/> of this treeview item
        ////        /// can be edit by the user or not.
        ////        /// </summary>
        ////        bool IsReadOnly { get; set; }
        #endregion properties

        #region methods
        /// <summary>
        /// Возвращает строковый путь либо:
        /// 1) для элемента <paramref name="current"/> или
        /// 2) для этого элемента (если не установлен необязательный параметр <paramref name="current"/>).
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        string GetStackPath(IItemModel current = null);
        #endregion methods
    }
}