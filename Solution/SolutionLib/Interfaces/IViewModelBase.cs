namespace SolutionLib.Interfaces
{
    using System.ComponentModel;

    /// <summary>
    /// Этот интерфейс предоставляет базовый интерфейс для всех элементов решения,
    /// включая корневой элемент, все элементы ниже него и корневой элемент решения.
    /// </summary>
    public interface IViewModelBase : INotifyPropertyChanged
    {
    }
}
