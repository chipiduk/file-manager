namespace SolutionLib.Interfaces
{
    /// <summary>
    /// Реализует интерфейс к модели представления файлового элемента в
    /// коллекции элементов с древовидной структурой. Типы элементов
    /// коллекции можно различать с помощью: 
    /// 1) интерфейса (например, для выбора шаблона в ItemTemplateSelector "/>
    /// или для использования в HierarchicalDataTemplate ./>, или 
    /// 2) перечисления в <см. Cref =" SolutionLib. Models.SolutionItemType "/>.
    /// </summary>
    public interface IFile : IItem
    {
    }
}
