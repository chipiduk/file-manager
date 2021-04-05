namespace SolutionLib.Interfaces
{
    using InplaceEditBoxLib.Events;
    using InplaceEditBoxLib.Interfaces;
    using SolutionLib.Models;
    using System.ComponentModel;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Определяет свойства и члены всех объектов, отображаемых в решении.
    /// </summary>
    public interface IItem : IEditBox, IViewModelBase, IParent
    {
        #region properties
        /// <summary>
        /// Получает уникальное техническое имя для 
        /// идентификации элемента и управления элементами в коллекции.
        /// </summary>
        SolutionItemType ItemType { get; }

        /// <summary>
        /// Получает имя для отображения в пользовательском интерфейсе.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Получает описание элемента - для использования во всплывающей подсказке и т. Д.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Получает / задает, развернут ли этот элемент древовидной структуры.
        /// </summary>
        bool IsItemExpanded { get; set; }

        /// <summary>
        ///Получает / задает, выбран ли этот элемент древовидного представления.
        /// </summary>
        bool IsItemSelected { get; set; }

        /// <summary>
        /// Получает, может ли пользователь редактировать
        /// <see cref="DisplayName"/> этого элемента дерева.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Получает/задает строку, определяющую порядок отображения элементов.
        /// </summary>
        string SortKey { get; set; }
        #endregion properties

        #region methods
        /// <summary>
        /// Устанавливает значение свойства <seealso cref = "DisplayName" />.
        /// </summary>
        /// <param name="displayName"></param>
        void SetDisplayName(string displayName);

        /// <summary>
        /// Устанавливает значение свойства <seealso cref = "Description" />.
        /// </summary>
        /// <param name="description"></param>
        void SetDescription(string description);

        /// <summary>
        /// Устанавливает значение свойства <seealso cref = "IsReadOnly" />.
        /// </summary>
        /// <param name="value"></param>
        void SetIsReadOnly(bool value);

        /// <summary>
        /// Устанавливает объект свойства Parent, где этот
        /// объект является дочерним в древовидном представлении.
        /// </summary>
        void SetParent(IItem parent);

        /// <summary>
        /// Sets the ID of an item in the collection.
        /// </summary>
        /// <param name="itemId"></param>
        void SetId(long itemId);

        /// <summary>
        /// Устанавливает ID элемента в коллекции.
        /// </summary>
        long GetId();

        #region IEditBox Members
        /// <summary>
        /// Вызовите этот метод, чтобы запросить запуск режима редактирования для переименования этого элемента.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Возвращает истину, если событие было успешно отправлено (слушатель прикреплен), в противном случае - ложь</returns>
        bool RequestEditMode(RequestEditEvent request);

        /// <summary>
        /// Показывает всплывающее уведомление с заданным заголовком и текстом.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="imageIcon"></param>
        /// <returns>true, если событие было успешно запущено.</returns>
        bool ShowNotification(string title, string message,
                                     BitmapImage imageIcon = null);
        #endregion IEditBox Members
        #endregion methods
    }
}
