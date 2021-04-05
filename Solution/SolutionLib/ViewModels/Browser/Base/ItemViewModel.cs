namespace SolutionLib.ViewModels.Browser.Base
{
    using InplaceEditBoxLib.Events;
    using SolutionLib.ViewModels.Base;
    using SolutionLib.Interfaces;
    using SolutionLib.Models;
    using System;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using UserNotification.Events;

    /// <summary>
    /// Реализует базовые функции для всех элементов древовидного представления,
    /// которые НЕ связаны с управлением дочерними коллекциями. Функциональные
    /// возможности этого базового класса сосредоточены на самом элементе
    /// (а не на его дочерних элементах). Дизайн нацелен на реализацию элементов,
    /// таких как файлы, которые могут даже не иметь дочерних элементов.
    /// </summary>
    internal abstract class ItemViewModel : BaseViewModel, IItem
    {
        #region fields
        private readonly SolutionItemType _ItemType;

        private string _DisplayName;
        private string _Description;
        private bool _IsItemExpanded;
        private bool _IsItemSelected;

        private IItem _Parent = null;

        private bool _IsReadOnly;
        private long _ItemId = -1;
        #endregion fields

        #region constructors
        /// <summary>
        /// Конструктор класса
        /// </summary>
        protected ItemViewModel(IItem parent, SolutionItemType itemType)
            : this()
        {
            SetParent(parent);
            _ItemType = itemType;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        protected ItemViewModel()
        {
            _IsItemExpanded = false;
            _IsItemSelected = false;

            _IsReadOnly = false;
        }
        #endregion constructors

        #region events
        /// <summary>
        /// Предоставьте событие, которое запускается, когда модель просмотра 
        /// сообщает свое представление: Вот еще одно сообщение с уведомлением,
        /// пожалуйста, покажите его пользователю.
        /// </summary>
        public event UserNotification.Events.ShowNotificationEventHandler ShowNotificationMessage;

        /// <summary>
        /// Предоставьте событие, которое запускается, когда модель просмотра запрашивает
        /// у своего представления запуск режима редактирования для переименования этого элемента.
        /// </summary>
        public event InplaceEditBoxLib.Events.RequestEditEventHandler RequestEdit;
        #endregion events

        #region properties
        /// <summary>
        /// Получает перечислимый тип элемента в решении.
        /// </summary>
        public SolutionItemType ItemType { get { return _ItemType; } }

        /// <summary>
        /// Получает имя этого элемента, которое будет отображаться в пользовательском интерфейсе.
        /// </summary>
        public string DisplayName
        {
            get { return _DisplayName; }
            private set
            {
                if (_DisplayName != value)
                {
                    _DisplayName = value;
                    NotifyPropertyChanged(() => DisplayName);
                }
            }
        }

        /// <summary>
        /// Получает описание этого элемента для отображения в пользовательском интерфейсе.
        /// </summary>
        public string Description
        {
            get { return _Description; }
            private set
            {
                if (_Description != value)
                {
                    _Description = value;
                    NotifyPropertyChanged(() => Description);
                }
            }
        }

        /// <summary>
        /// Получает / задает, развернут ли этот элемент древовидной структуры.
        /// </summary>
        public bool IsItemExpanded
        {
            get { return _IsItemExpanded; }
            set
            {
                if (_IsItemExpanded != value)
                {
                    _IsItemExpanded = value;
                    NotifyPropertyChanged(() => IsItemExpanded);
                }
            }
        }

        /// <summary>
        /// Получает / устанавливает, выбран ли этот элемент древовидного представления.
        /// </summary>
        public bool IsItemSelected
        {
            get { return _IsItemSelected; }
            set
            {
                if (_IsItemSelected != value)
                {
                    _IsItemSelected = value;
                    NotifyPropertyChanged(() => IsItemSelected);
                }
            }
        }

        /// <summary>
        /// Получает / задает, может ли пользователь переименовать 
        /// <see cref = "DisplayName" /> этого элемента древовидного представления.
        /// 
        /// Это свойство является частью интерфейса 
        /// <see cref = "InplaceEditBoxLib.Interfaces.IEditBox" />, поэтому его не следует переименовывать.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            private set
            {
                if (_IsReadOnly != value)
                {
                    _IsReadOnly = value;
                    NotifyPropertyChanged(() => IsReadOnly);
                }
            }
        }

        /// <summary>
        /// Получает родительский объект, в котором этот объект является дочерним в древовидном представлении.
        /// </summary>
        public IItem Parent { get { return _Parent; } }

        /// <summary>
        /// Получает / задает строку, определяющую порядок отображения элементов.
        /// </summary>
        public string SortKey { get; set; }
        #endregion properties

        #region methods
        /// <summary>
        /// Устанавливает значение свойства <seealso cref = "DisplayName" />.
        /// </summary>
        /// <param name="displayName"></param>
        public void SetDisplayName(string displayName)
        {
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Устанавливает значение свойства <seealso cref = "Description" />.
        /// </summary>
        /// <param name="description"></param>
        public void SetDescription(string description)
        {
            this.Description = description;
        }

        /// <summary>
        /// Устанавливает значение свойства <seealso cref = "IsReadOnly" />.
        /// </summary>
        /// <param name="value"></param>
        public void SetIsReadOnly(bool value)
        {
            IsReadOnly = value;
        }

        /// <summary>
        /// Устанавливает объект свойства <see cref = "Parent" />,
        /// где этот объект является дочерним в древовидном представлении.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(IItem parent)
        {
            _Parent = parent;
            NotifyPropertyChanged(() => Parent);
        }

        /// <summary>
        /// Устанавливает идентификатор элемента в коллекции.
        /// </summary>
        /// <param name="itemId"></param>
        void IItem.SetId(long itemId)
        {
            _ItemId = itemId;
        }

        /// <summary>
        /// Получает идентификатор элемента в коллекции.
        /// </summary>
        long IItem.GetId()
        {
            return _ItemId;
        }

        #region IEditBox Members
        /// <summary>
        /// Вызовите этот метод, чтобы запросить запуск режима редактирования
        /// для переименования этого элемента.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Возвращает true, если событие было успешно отправлено (слушатель прикреплен), в противном случае - false.</returns>
        public bool RequestEditMode(RequestEditEvent request)
        {
            if (this.RequestEdit != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.RequestEdit(this, new RequestEdit(request));
                }));
                return true;
            }
            else
            {
                System.Console.WriteLine("CANNOT Request Edit Mode in ViewModel (No View Attached).");
            }

            return false;
        }

        /// <summary>
        /// Показывает всплывающее уведомление с заданным заголовком и текстом.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="imageIcon"></param>
        /// <returns>true, если событие было успешно запущено</returns>
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
        #endregion IEditBox Members
        #endregion methods
    }
}
