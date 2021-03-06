namespace InplaceEditBoxLib.Views
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Реализует поведение, которое можно использовать для прикрепления входных привязок к стилю (например, TreeViewItem).
    /// https://stackoverflow.com/questions/2660760/defining-inputbindings-within-a-style#7808997
    /// </summary>
    public class Attach
    {
        private static readonly DependencyProperty InputBindingsProperty =
            DependencyProperty.RegisterAttached("InputBindings",
                typeof(InputBindingCollection),
                typeof(Attach),
                new FrameworkPropertyMetadata(new InputBindingCollection(),
                (sender, e) =>
                {
                    var element = sender as UIElement;
                    if (element == null) return;

                    element.InputBindings.Clear();
                    element.InputBindings.AddRange((InputBindingCollection)e.NewValue);
                }));

        /// <summary>
        /// Получает свойство зависимости «InputBindings».
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static InputBindingCollection GetInputBindings(UIElement element)
        {
            return (InputBindingCollection)element.GetValue(InputBindingsProperty);
        }

        /// <summary>
        /// Устанавливает свойство зависимости «InputBindings».
        /// </summary>
        public static void SetInputBindings(UIElement element, InputBindingCollection inputBindings)
        {
            element.SetValue(InputBindingsProperty, inputBindings);
        }
    }
}
