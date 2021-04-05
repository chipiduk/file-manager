namespace SolutionLib
{
    using SolutionLib.Interfaces;
    using SolutionLib.ViewModels.Browser;

    /// <summary>
    /// Содержит методы для создания библиотечных объектов, которые доступны только через интерфейс.
    /// </summary>
    public sealed class Factory
    {
        private Factory()
        {
        }

        /// <summary>
        /// Получает корень объекта модели представления дерева решений. 
        /// Используйте функции, доступные ниже <see cref = "ISolution" />,
        /// чтобы управлять элементами в этом дереве.
        /// </summary>
        /// <returns></returns>
        public static ISolution RootViewModel()
        {
            return new SolutionViewModel();
        }
    }
}
