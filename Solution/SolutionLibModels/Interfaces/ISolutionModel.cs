namespace SolutionModelsLib.Interfaces
{
    using SolutionModelsLib.Enums;
    using System.Xml.Serialization;

    /// <summary>
    /// Реализует интерфейс для корневого класса модели, 
    /// который управляет всей структурой данных древовидной 
    /// модели, которая в основном используется для чтения и 
    /// записи данных из и в постоянство на основе файлов.
    /// </summary>
    public interface ISolutionModel : IModelBase, IXmlSerializable
    {
        /// <summary>
        /// Получает корневой элемент древовидной структуры,
        /// управляемой реализующим объектом (это дерево имеет
        /// только один корневой элемент, поэтому здесь у нас нет коллекции).
        /// </summary>
        ISolutionRootItemModel Root { get; set; }

        /// <summary>
        /// Добавляет новый элемент запрошенного типа и имени по по заданным параметрам
        /// <paramref name="parent"/> item.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="itemType"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        IItemModel AddChild(string itemName,
                            SolutionModelItemType itemType,
                            IItemChildrenModel parent);

        /// <summary>
        /// Добавляет дочерний элемент заданного типа
        /// (<see cref="SolutionItemType.SolutionRootItem"/> нельзя добавить сюда).
        /// 
        /// Эта оболочка использует длинный ввод для преобразования при чтении из файла.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IItemModel AddChild(string displayName
                          , long longType
                          , IItemChildrenModel parent);

        /// <summary>
        /// Создает новый корневой элемент решения из заданных параметров 
        /// (заменяет текущий корневой элемент, если он есть) и возвращает его интерфейс.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        IItemChildrenModel AddSolutionRootItem(string displayName, long id = -1);
    }
}