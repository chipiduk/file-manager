namespace SolutionModelsLib.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Реализует интерфейс для абстрактного класса, который реализует
    /// элементы, которые могут иметь собственные дочерние элементы.
    /// </summary>
    public interface IItemChildrenModel : IItemModel
    {
        #region properties
        /// <summary>
        /// Получает все дочерние элементы этого (родительского) элемента.
        /// </summary>
        IList<IItemModel> Children { get; }
        #endregion properties

        #region methods
        /// <summary>
        /// Находит дочерний элемент на основе заданного ключа в <paramref name="displayName"/>.    
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IItemModel FindChild(string displayName);

        /// <summary>
        /// Добавляет дочерний элемент заданного типа
        /// (<see cref="SolutionItemType.SolutionRootItem"/> нельзя добавить здесь).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        void AddChild(IItemModel item);
        #endregion methods
    }
}
