namespace InplaceEditBoxLib.Interfaces
{
    using InplaceEditBoxLib.Events;
    using UserNotification.Interfaces;

    /// <summary>
    /// Реализовать интерфейс, который позволяет модели просмотра 
    /// взаимодействовать с элементом управления <seealso cref="InplaceEditBoxLib.Views.EditBox"/>.
    /// </summary>
    public interface IEditBox : INotifyableViewModel
    {
        /// <summary>
        /// Модель просмотра может инициировать это событие, чтобы запросить редактирование
        /// имени элемента, чтобы запустить процесс переименования с помощью элемента управления
        /// <seealso cref="InplaceEditBoxLib.Views.EditBox"/>. Элемент управления запустит команду,
        /// привязанную к свойству зависимости команды (если есть) с новым именем в качестве параметра
        /// (если редактирование не было отменено (через escape) тем временем.
        /// </summary>
        event RequestEditEventHandler RequestEdit;
    }
}
