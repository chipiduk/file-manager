namespace SolutionLib.Views
{
    using SolutionLib.Interfaces;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Реализует селектор DataTemplate для TreeViewItems. Это необходимо,
    /// поскольку записи файлов НЕ МОГУТ иметь дочерние элементы, а все
    /// другие типы элементов могут иметь дочерние записи под собой.
    /// 
    ///   Использование: 
    ///   - Создание экземпляра селектора шаблона в ResourceDictionary 
    ///   - Создание экземпляра: 
    ///   - DataTemplate (для файлов) и 
    ///   - HierarchicalDataTemplate (элементы с дочерними элементами) в ResourceDictionary
    ///   
    ///   - Назначьте каждый шаблон для свойств <see cref = "FileTemplate" />
    ///   и <see cref = "ChildrenItemTemplate" /> ниже.
    ///   
    /// - Назначьте <see Cref = "TreeViewItemSelector" /> объекту TreeView ItemTemplateSelector = "{StaticResource TreeItemSelector}"
    /// </summary>
    public class TreeViewItemSelector : DataTemplateSelector
    {
        /// <summary>
        /// Получает / задает свойство, содержащее шаблон для элементов,
        /// которые не могут иметь дочерних элементов (файлов) в TreeView.
        /// </summary>
        public DataTemplate FileTemplate { get; set; }

        /// <summary>
        /// Получает / задает свойство, содержащее шаблон для элементов,
        /// которые могут иметь дочерние элементы (папка, проект) в TreeView.
        /// </summary>
        public DataTemplate ChildrenItemTemplate { get; set; }

        /// <summary>
        /// Переопределяет стандартный метод, который вызывается, когда платформа
        /// запрашивает правильный шаблон, который будет использоваться для данного
        /// объекта ViewModel.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IFile)
                return FileTemplate;

            return ChildrenItemTemplate;
        }
    }
}
