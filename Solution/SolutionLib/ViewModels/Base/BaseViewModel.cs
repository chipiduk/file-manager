namespace SolutionLib.ViewModels.Base
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

    /// <summary>
    /// ������ ����� ViewModel ��������� ��� ���������� ���������� INotifyPropertyChanged,
    /// ����� �������� WPF �� ��������� �������� (��������, ��� ���������� ������ ��� �����������).
    /// 
    /// �������������, ����� PropertyChanged ������ ���������� ��� ��������� ������, ������ ���
    /// ��������������� �������� ����� ��� �� ����� ���� ��������� � ��������� ������������ ����������,
    /// �������, � ���� �������, ������ ��������� ���� �����������.
    /// 
    /// ����� PropertyChanged ������ ���������� ������� � ���������� ������, ������������ �� ����� ������.
    /// ������ ����� �������� ��� ��������, ������� ���������� ��������.
    /// 
    /// BaseViewModel �������� ����������� �� System.Windows.DependencyObject, ����� ���������
    /// �������������� ViewModel ����������� �������� ������������. �������� ������������, � ���� �������,
    /// ������� ��� ������ � IValueConverter � ConverterParameters.
    /// </summary>
    internal class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// ����������� ���������� ������� ���������� <seealso cref = "INotifyPropertyChanged" />.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// �������� ��������� ��������� ���������� (����� �������� WPF), ����� ��� ��������� �� �����������.
        /// 
        /// ������ ������: this.NotifyPropertyChanged (() => this.IsSelected);
        /// ��� "this" ���������� �� <seealso cref = "BaseViewModel" />
        /// � IsSelected - ��� ��������.
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
        /// �������� ��������� ��������� ���������� (����� �������� WPF), ����� ��� ��������� �� �����������.
        /// 
        /// ������ ������: this.OnPropertyChanged ("IsSelected");
        /// ��� "this" ���������� �� <seealso cref = "BaseViewModel" />
        /// � IsSelected - ��� ��������.
        /// </summary>
        /// <param name="propertyName">�������� �������� ��� ����������</param>
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
