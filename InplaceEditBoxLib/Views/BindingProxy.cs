namespace InplaceEditBoxLib.Views
{
    using System.Windows;

    /// <summary>
    /// Реализует прокси XAML, который можно использовать для привязки элементов (TreeViewItem,
    /// ListViewItem и т. Д.) К модели просмотра, которая управляет коллекциями.
    /// 
    /// Source: http://www.thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/
    ///  Issue: http://stackoverflow.com/questions/9994241/mvvm-binding-command-to-contextmenu-item
    /// </summary>
    public class BindingProxy : Freezable
    {
        /// <summary>
        /// Резервное хранилище свойства зависимости данных.
        ///
        /// Получает / устанавливает объект данных, который этот класс
        /// пересылает всем, у кого есть ссылка на этот объект.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        /// <summary>
        /// Получает / устанавливает объект данных, который этот класс
        /// пересылает всем, у кого есть ссылка на этот объект.
        /// </summary>
        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// Переопределения Freezable
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}
