namespace ServiceLocator
{
    using ExplorerLib;
    using ServiceLocator;

    /// <summary>
    /// Создает и инициализирует все службы.
    /// </summary>
    public static class ServiceInjector
    {
        /// <summary>
        /// Загружает сервисные объекты в ServiceContainer при запуске.
        /// </summary>
        /// <returns>Возвращает текущий экземпляр <seealso cref = "ServiceContainer" />,
        /// чтобы позволить вызывающему объекту работать с элементами контейнера служб
        /// сразу после создания.</returns>
        public static ServiceContainer InjectServices()
        {
            ServiceContainer.Instance.AddService<IExplorer>(new Explorer());

            return ServiceContainer.Instance;
        }
    }
}
