namespace InplaceEditBoxLib.Events
{
  using System;

    /// <summary>
    /// Определите тип события редактирования, которое может быть запрошено из представления.
    /// </summary>
    public enum RequestEditEvent
  {
        /// <summary>
        /// Запустите режим редактирования для переименования элемента, представленного
        /// моделью просмотра, которая отправляет это сообщение своему представлению.
        /// </summary>
        StartEditMode,

        /// <summary>
        /// Неизвестный тип события никогда не должен происходить, если это перечисление используется правильно.
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Метод делегирования обработчика событий, который будет использоваться при обработке событий <seealso cref = "RequestEdit" />.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RequestEditEventHandler(object sender, RequestEdit e);

    /// <summary>
    /// Реализует событие, которое может быть отправлено из модели
    /// просмотра для запроса определенных режимов редактирования.
    /// </summary>
    public class RequestEdit : EventArgs
  {
        #region constructors
        /// <summary>
        /// Конструктор параметризованного класса
        /// </summary>
        /// <param name="eventRequest"></param>
        public RequestEdit(RequestEditEvent eventRequest) : this()
    {
      this.Request = eventRequest;
    }

    /// <summary>
    /// Конструктор класса
    /// </summary>
    protected RequestEdit()
    {
      this.Request = RequestEditEvent.Unknown;
    }
        #endregion constructors

        /// <summary>
        /// Получает тип события редактирования, запрошенного моделью просмотра.
        /// </summary>
        public RequestEditEvent Request { get; private set; }
  }
}
