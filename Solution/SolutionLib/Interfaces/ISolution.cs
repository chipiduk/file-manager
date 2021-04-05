namespace SolutionLib.Interfaces
{
    using SolutionLib.Models;
    using System.Collections.Generic;
    using System.Windows.Input;

    /// <summary>
    /// Корень решения - это класс, в котором размещены все другие элементы, связанные с
    /// решением. Даже SolutionRootItem, который является частью отображаемой коллекции,
    /// размещается в коллекции ниже.
    /// </summary>
    public interface ISolution : IViewModelBase
    {
        #region properties
        /// <summary>
        /// Получает корень древовидной структуры. То есть в ObservableCollection
        /// есть только 1 элемент, и этот элемент является корневым.
        /// 
        /// Свойство Children этого <see cref="IItemChildren"/>
        /// представляет остальную часть дерева.
        /// </summary>
        IEnumerable<IItem> Root { get; }

        /// <summary>
        /// Получает команду, которая переименовывает элемент, представленный этой моделью
        /// просмотра. Эта команда должна вызываться непосредственно реализующим представлением,
        /// поскольку новое имя элемента доставляется в виде строки с самим элементом в качестве
        /// второго параметра через привязку через свойство зависимостей RenameCommandParameter.
        /// </summary>
        ICommand RenameCommand { get; }

        /// <summary>
        /// Получает текущий выбранный элемент из коллекции элементов дерева.
        /// </summary>
        IItem SelectedItem { get; }

        /// <summary>
        /// Получает команду, которая изменяет текущий <see cref="SelectedItem"/>
        /// на элемент, который предоставляется как параметр <see cref="IItem"/>
        /// этой команды.
        /// </summary>
        ICommand SelectionChangedCommand { get; }

        /// <summary>
        /// Получает фильтр файлов, который применяется, когда пользователь открывает
        /// диалоговое окно сохранения/загрузки для сохранения/загрузки содержимого
        /// древовидного представления решения.
        /// </summary>
        string SolutionFileFilter { get; }

        /// <summary>
        /// Получает фильтр файлов по умолчанию, который применяется, когда пользователь
        /// открывает диалоговое окно сохранения / загрузки для сохранения / загрузки 
        /// содержимого древовидного представления решения в первый раз.
        /// </summary>
        string SolutionFileFilterDefault { get; }
        #endregion properties

        #region methods
        /// <summary>
        /// Возвращает первый видимый элемент в древовидной структуре (если есть) или null.
        /// Этот метод представляет собой удобную оболочку, которая разворачивает свойство 
        /// <see cref="Root"/>, поскольку модель просмотра всегда поддерживает только ОДИН корень.
        /// </summary>
        /// <returns></returns>
        IItemChildren GetRootItem();

        /// <summary>
        /// Добавляет корень решения в коллекцию элементов решения. Будьте осторожны (!),
        /// Так как текущий корневой элемент (если есть) отбрасывается вместе со всеми его
        /// дочерними элементами, поскольку модель просмотра всегда поддерживает только ОДИН корень.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IItemChildren AddSolutionRootItem(string displayName);

        /// <summary>
        /// Добавляет еще один дочерний элемент ниже корневого элемента
        /// в коллекцию. Это вызовет исключение, если родительский элемент равен нулю.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        IItem AddRootChild(string itemName,
                                SolutionItemType itemType);

        /// <summary>
        /// Добавляет другой файловый (дочерний) элемент под родительским элементом.
        /// Это вызовет исключение, если родительский элемент равен нулю.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="parent"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        IItem AddChild(string itemName,
                            SolutionItemType itemType,
                            IItemChildren parent);

        /// <summary>
        /// Resets all viewmodel items to initial states of construction time.
        /// Сбросить все модели представления элементов в первоначальное состояние, определяемое конструктором.
        /// </summary>
        void ResetToDefaults();
        #endregion methods
    }
}
