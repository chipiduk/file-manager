namespace SolutionModelsLib
{
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models;

    /// <summary>
    /// Класс создает объекты модели для внешнего мира.
    /// </summary>
    public sealed class Factory
    {
        private Factory()
        {
        }

        /// <summary>
        /// Создайте объект корневой модели данных решения и возвращает его.
        /// </summary>
        /// <returns></returns>
        public static ISolutionModel CreateSolutionModel()
        {
            return new SolutionModel();
        }
    }
}
