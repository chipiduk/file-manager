namespace SolutionLib.ViewModels.Base
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

    /// <summary>
    /// Каждый класс ViewModel требуется для реализации интерфейса INotifyPropertyChanged,
    /// чтобы сообщать WPF об изменении свойства (например, при выполнении метода или установщика).
    /// 
    /// Следовательно, метод PropertyChanged должен вызываться при изменении данных, потому что
    /// соответствующие свойства могут или не могут быть привязаны к элементам графического интерфейса,
    /// которые, в свою очередь, должны обновлять свое отображение.
    /// 
    /// Метод PropertyChanged должен вызываться членами и свойствами класса, производного от этого класса.
    /// Каждый вызов содержит имя свойства, которое необходимо обновить.
    /// 
    /// BaseViewModel является производным от System.Windows.DependencyObject, чтобы позволить
    /// результирующим ViewModel реализовать свойства зависимостей. Свойства зависимостей, в свою очередь,
    /// полезны при работе с IValueConverter и ConverterParameters.
    /// </summary>
    internal class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Стандартный обработчик событий интерфейса <seealso cref = "INotifyPropertyChanged" />.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Сообщите связанным элементам управления (через привязку WPF), чтобы они обновляли их отображение.
        /// 
        /// Пример вызова: this.NotifyPropertyChanged (() => this.IsSelected);
        /// где "this" происходит от <seealso cref = "BaseViewModel" />
        /// а IsSelected - это свойство.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="property"></param>
        public void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
                memberExpression = (MemberExpression)lambda.Body;

            this.OnPropertyChanged(memberExpression.Member.Name);
        }

        /// <summary>
        /// Сообщите связанным элементам управления (через привязку WPF), чтобы они обновляли их отображение.
        /// 
        /// Пример вызова: this.OnPropertyChanged ("IsSelected");
        /// где "this" происходит от <seealso cref = "BaseViewModel" />
        /// а IsSelected - это свойство.
        /// </summary>
        /// <param name="propertyName">Название свойства для обновления</param>
        public void OnPropertyChanged(string propertyName)
        {
            try
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            catch
            {
            }
        }
    }
}
