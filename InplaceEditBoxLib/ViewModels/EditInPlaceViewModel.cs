namespace InplaceEditBoxLib.ViewModels
{
    using System.Windows.Media.Imaging;
    using InplaceEditBoxLib.Events;
    using InplaceEditBoxLib.Interfaces;
    using UserNotification.Events;

    /// <summary>
    /// Реализует класс модели просмотра, который может использоваться в качестве
    /// основы модели просмотра, управляющей представлением <seealso cref = "InplaceEditBoxLib.Views.EditBox" />.
    /// </summary>
    public class EditInPlaceViewModel : Base.ViewModelBase, IEditBox
    {
        #region fields
        /// <summary>
        /// Средство ведения журнала Log4net для этого класса.
        /// </summary>
        protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool _IsReadOnly;
        #endregion fields

        #region events
        /// <summary>
        /// Предоставьте событие, которое запускается, когда модель просмотра
        /// сообщает свое представление: Вот еще одно сообщение с уведомлением,
        /// пожалуйста, покажите его пользователю.
        /// </summary>
        public event UserNotification.Events.ShowNotificationEventHandler ShowNotificationMessage;

        /// <summary>
        /// Предоставьте событие, которое запускается, когда модель просмотра 
        /// запрашивает у своего представления запуск режима редактирования для переименования этого элемента.
        /// </summary>
        public event InplaceEditBoxLib.Events.RequestEditEventHandler RequestEdit;
        #endregion events

        #region constructors
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public EditInPlaceViewModel()
        {
            this.IsReadOnly = false;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Получает / устанавливает, доступна ли эта папка только для чтения
        /// (может быть переименована) или нет. Например, диск нельзя переименовать,
        /// и поэтому он доступен только для чтения в этом контексте.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this._IsReadOnly;
            }

            protected set
            {
                if (this._IsReadOnly != value)
                {
                    this._IsReadOnly = value;

                    this.RaisePropertyChanged(() => this.IsReadOnly);
                }
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Вызовите этот метод, чтобы запросить запуск режима редактирования для переименования этого элемента.
        /// </summary>
        /// <param name="request"></param>
        /// <returns> Возвращает истину, если событие было успешно отправлено (слушатель прикреплен), в противном случае - ложь. </returns>
        public bool RequestEditMode(RequestEditEvent request)
        {
            if (this.RequestEdit != null)
            {
                this.RequestEdit(this, new RequestEdit(request));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Показывает всплывающее уведомление с заданным заголовком и текстом.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="imageIcon"></param>
        /// <returns>истина, если событие было успешно запущено.</returns>
        public bool ShowNotification(string title, string message,
                                     BitmapImage imageIcon = null)
        {
            if (this.ShowNotificationMessage != null)
            {
                this.ShowNotificationMessage(this, new ShowNotificationEvent
                (
                  title,
                  message,
                  imageIcon
                ));

                return true;
            }

            return false;
        }
        #endregion methods
    }
}
