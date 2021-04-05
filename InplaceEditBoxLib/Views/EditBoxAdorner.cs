namespace InplaceEditBoxLib.Views
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Класс украшения, который содержит TextBox, чтобы предоставить возможность
    /// редактирования для элемента управления EditBox. Редактируемый TextBox находится
    /// в AdornerLayer. Когда EditBox находится в режиме редактирования, TextBox получает
    /// желаемый размер; в противном случае расположите его по размеру (0,0,0,0).
    /// 
    /// В этом коде использовалась часть работы команды ATC Avalon.
    /// (http://blogs.msdn.com/atc_avalon_team/archive/2006/03/14/550934.aspx)
    /// </summary>
    internal sealed class EditBoxAdorner : Adorner
    {
        #region fields
        /// <summary>
        /// Дополнительное заполнение для содержимого, когда оно отображается в TextBox
        /// </summary>
        private const double ExtraWidth = 15;

        /// <summary>
        /// Визуальные дети
        /// </summary>
        private VisualCollection _VisualChildren;

        /// <summary>
        /// Элемент управления, содержащий элементы управления Adorned
        /// и Adorner. Эта ссылка требуется для вычисления ширины
        /// окружающего средства просмотра прокрутки.
        /// </summary>
        private EditBox _EditBox;

        /// <summary>
        /// Текстовое поле, которое закрывает этот декоративный элемент.
        /// </summary>
        private TextBox _TextBox;

        /// <summary>
        /// Находится ли EditBox в режиме редактирования, что означает, что Adorner виден.
        /// </summary>
        private bool _IsVisible;

        /// <summary>
        /// Canvas, содержащий TextBox, который обеспечивает возможность отображения
        /// большего размера, чем текущий размер ячейки, так что все содержимое ячейки
        /// может быть отредактировано
        /// </summary>
        private Canvas _Canvas;

        /// <summary>
        /// Максимальный размер текстового поля в зависимости от окружающего средства просмотра
        /// прокрутки вычисляется по запросу в методе измерения и становится недействительным при изменении видимости Adorner.
        /// </summary>
        private double _TextBoxMaxWidth = double.PositiveInfinity;
        #endregion fields

        #region constructor
        /// <summary>
        /// Inialize the EditBoxAdorner.
        /// 
        ///   +---> adorningElement (TextBox)
        ///   |
        /// adornedElement (TextBlock)
        /// </summary>
        /// <param name="adornedElement"></param>
        /// <param name="adorningElement"></param>
        /// <param name="editBox"></param>
        public EditBoxAdorner(UIElement adornedElement,
                              TextBox adorningElement,
                              EditBox editBox)
          : base(adornedElement)
        {
            _TextBox = adorningElement;
            Debug.Assert(_TextBox != null, "No TextBox!");

            _VisualChildren = new VisualCollection(this);

            this.BuildTextBox(editBox);
        }
        #endregion constructor

        #region Properties
        /// <summary>
        ///override для возврата информации о визуальном дереве.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return _VisualChildren.Count; }
        }

        /// <summary>
        /// Получает, отображается ли текстовое поле в данный момент или нет.
        /// </summary>
        public bool IsTextBoxVisible
        {
            get
            {
                return _IsVisible;
            }
        }
        #endregion Properties

        #region methods
        /// <summary>
        /// Указывает, отображается ли TextBox 
        /// при изменении свойства IsEditing.
        /// </summary>
        /// <param name="isVisible"></param>
        public void UpdateVisibilty(bool isVisible)
        {
            _IsVisible = isVisible;
            InvalidateMeasure();
            _TextBoxMaxWidth = double.PositiveInfinity;
        }

        /// <summary>
        /// Функция переопределения для возврата информации о визуальном дереве.
        /// </summary>
        protected override Visual GetVisualChild(int index)
        {
            return _VisualChildren[index];
        }

        /// <summary>
        /// Функция переопределения для упорядочивания элементов.
        /// </summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_IsVisible)
            {
                _TextBox.Arrange(new Rect(-1, -1, finalSize.Width, finalSize.Height));
            }
            else // если нет редактируемого режима, нет необходимости показывать элементы.
            {
                _TextBox.Arrange(new Rect(0, 0, 0, 0));
            }

            return finalSize;
        }

        /// <summary>
        /// Переопределить для измерения элементов.
        /// </summary>
        protected override Size MeasureOverride(Size constraint)
        {
            _TextBox.IsEnabled = _IsVisible;

            // в режиме редактирования измерьте пространство, которое должен занимать элемент украшения.
            if (_IsVisible == true)
            {
                if (double.IsInfinity(_TextBoxMaxWidth) == true)
                {
                    Point position = _TextBox.PointToScreen(new Point(0, 0)),
                    controlPosition = _EditBox.ParentScrollViewer.PointToScreen(new Point(0, 0));

                    position.X = Math.Abs(controlPosition.X - position.X);
                    position.Y = Math.Abs(controlPosition.Y - position.Y);

                    _TextBoxMaxWidth = _EditBox.ParentScrollViewer.ViewportWidth - position.X;
                }

                if (this.AdornedElement.Visibility == System.Windows.Visibility.Collapsed)
                    return new Size(_TextBoxMaxWidth, _TextBox.DesiredSize.Height);

                // 
                if (constraint.Width > _TextBoxMaxWidth)
                {
                    constraint.Width = _TextBoxMaxWidth;
                }

                AdornedElement.Measure(constraint);
                _TextBox.Measure(constraint);

                double desiredWidth = AdornedElement.DesiredSize.Width + ExtraWidth;

                // Поскольку украшение должно закрывать EditBox, он должен возвращать
                // AdornedElement.Width, дополнительные 15 должны сделать его более понятным.

                if (desiredWidth < _TextBoxMaxWidth)
                    return new Size(desiredWidth, _TextBox.DesiredSize.Height);
                else
                {
                    this.AdornedElement.Visibility = System.Windows.Visibility.Collapsed;

                    return new Size(_TextBoxMaxWidth, _TextBox.DesiredSize.Height);
                }
            }
            else  // не нужно ничего показывать, если он не находится в редактируемом режиме.
                return new Size(0, 0);
        }

        /// <summary>
        /// Инициализируйте необходимые свойства и перехватите необходимые
        /// события в TextBox, затем добавьте его в дерево.
        /// </summary>
        private void BuildTextBox(EditBox editBox)
        {
            _EditBox = editBox;

            _Canvas = new Canvas();
            _Canvas.Children.Add(_TextBox);
            _VisualChildren.Add(_Canvas);

            // Привязать TextBox к свойству элемента управления Edit Box Text
            Binding binding = new Binding("Text");
            binding.Source = editBox;
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            _TextBox.SetBinding(TextBox.TextProperty, binding);

            // Привязать текст к тексту свойства AdornedElement
            binding = new Binding("Text");
            binding.Source = this.AdornedElement;
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            _TextBox.SetBinding(TextBox.TextProperty, binding);
        }
        #endregion methods
    }
}
