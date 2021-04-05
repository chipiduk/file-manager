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
    /// ����� ���������, ������� �������� TextBox, ����� ������������ �����������
    /// �������������� ��� �������� ���������� EditBox. ������������� TextBox ���������
    /// � AdornerLayer. ����� EditBox ��������� � ������ ��������������, TextBox ��������
    /// �������� ������; � ��������� ������ ����������� ��� �� ������� (0,0,0,0).
    /// 
    /// � ���� ���� �������������� ����� ������ ������� ATC Avalon.
    /// (http://blogs.msdn.com/atc_avalon_team/archive/2006/03/14/550934.aspx)
    /// </summary>
    internal sealed class EditBoxAdorner : Adorner
    {
        #region fields
        /// <summary>
        /// �������������� ���������� ��� �����������, ����� ��� ������������ � TextBox
        /// </summary>
        private const double ExtraWidth = 15;

        /// <summary>
        /// ���������� ����
        /// </summary>
        private VisualCollection _VisualChildren;

        /// <summary>
        /// ������� ����������, ���������� �������� ���������� Adorned
        /// � Adorner. ��� ������ ��������� ��� ���������� ������
        /// ����������� �������� ��������� ���������.
        /// </summary>
        private EditBox _EditBox;

        /// <summary>
        /// ��������� ����, ������� ��������� ���� ������������ �������.
        /// </summary>
        private TextBox _TextBox;

        /// <summary>
        /// ��������� �� EditBox � ������ ��������������, ��� ��������, ��� Adorner �����.
        /// </summary>
        private bool _IsVisible;

        /// <summary>
        /// Canvas, ���������� TextBox, ������� ������������ ����������� �����������
        /// �������� �������, ��� ������� ������ ������, ��� ��� ��� ���������� ������
        /// ����� ���� ���������������
        /// </summary>
        private Canvas _Canvas;

        /// <summary>
        /// ������������ ������ ���������� ���� � ����������� �� ����������� �������� ���������
        /// ��������� ����������� �� ������� � ������ ��������� � ���������� ���������������� ��� ��������� ��������� Adorner.
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
        ///override ��� �������� ���������� � ���������� ������.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return _VisualChildren.Count; }
        }

        /// <summary>
        /// ��������, ������������ �� ��������� ���� � ������ ������ ��� ���.
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
        /// ���������, ������������ �� TextBox 
        /// ��� ��������� �������� IsEditing.
        /// </summary>
        /// <param name="isVisible"></param>
        public void UpdateVisibilty(bool isVisible)
        {
            _IsVisible = isVisible;
            InvalidateMeasure();
            _TextBoxMaxWidth = double.PositiveInfinity;
        }

        /// <summary>
        /// ������� ��������������� ��� �������� ���������� � ���������� ������.
        /// </summary>
        protected override Visual GetVisualChild(int index)
        {
            return _VisualChildren[index];
        }

        /// <summary>
        /// ������� ��������������� ��� �������������� ���������.
        /// </summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_IsVisible)
            {
                _TextBox.Arrange(new Rect(-1, -1, finalSize.Width, finalSize.Height));
            }
            else // ���� ��� �������������� ������, ��� ������������� ���������� ��������.
            {
                _TextBox.Arrange(new Rect(0, 0, 0, 0));
            }

            return finalSize;
        }

        /// <summary>
        /// �������������� ��� ��������� ���������.
        /// </summary>
        protected override Size MeasureOverride(Size constraint)
        {
            _TextBox.IsEnabled = _IsVisible;

            // � ������ �������������� �������� ������������, ������� ������ �������� ������� ���������.
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

                // ��������� ��������� ������ ��������� EditBox, �� ������ ����������
                // AdornedElement.Width, �������������� 15 ������ ������� ��� ����� ��������.

                if (desiredWidth < _TextBoxMaxWidth)
                    return new Size(desiredWidth, _TextBox.DesiredSize.Height);
                else
                {
                    this.AdornedElement.Visibility = System.Windows.Visibility.Collapsed;

                    return new Size(_TextBoxMaxWidth, _TextBox.DesiredSize.Height);
                }
            }
            else  // �� ����� ������ ����������, ���� �� �� ��������� � ������������� ������.
                return new Size(0, 0);
        }

        /// <summary>
        /// ��������������� ����������� �������� � ����������� �����������
        /// ������� � TextBox, ����� �������� ��� � ������.
        /// </summary>
        private void BuildTextBox(EditBox editBox)
        {
            _EditBox = editBox;

            _Canvas = new Canvas();
            _Canvas.Children.Add(_TextBox);
            _VisualChildren.Add(_Canvas);

            // ��������� TextBox � �������� �������� ���������� Edit Box Text
            Binding binding = new Binding("Text");
            binding.Source = editBox;
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            _TextBox.SetBinding(TextBox.TextProperty, binding);

            // ��������� ����� � ������ �������� AdornedElement
            binding = new Binding("Text");
            binding.Source = this.AdornedElement;
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            _TextBox.SetBinding(TextBox.TextProperty, binding);
        }
        #endregion methods
    }
}
