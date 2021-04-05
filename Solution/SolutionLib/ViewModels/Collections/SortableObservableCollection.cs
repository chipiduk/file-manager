namespace SolutionLib.ViewModels.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Source: https://stackoverflow.com/questions/5487927/expand-wpf-treeview-to-support-sorting
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortableObservableCollection<T> : ObservableCollection<T>
    {
        private static DispatcherPriority _ChildrenEditPrio = DispatcherPriority.DataBind;

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public SortableObservableCollection() : base() { }

        /// <summary>
        /// Class constructor
        /// </summary>
        public SortableObservableCollection(List<T> l) : base(l) { }

        /// <summary>
        /// Class constructor
        /// </summary>
        public SortableObservableCollection(IEnumerable<T> l) : base(l) { }
        #endregion

        #region Sorting
        /// <summary>
        /// Сортирует элементы коллекции в порядке возрастания в соответствии с ключом.
        /// </summary>
        /// <typeparam name="TKey"> Тип ключа, возвращаемый <paramref name="keySelector"/>.</typeparam>
        /// <param name="keySelector">Функция для извлечения ключа из элемента.</param>
        public void Sort<TKey>(Func<T, TKey> keySelector)
        {
            InternalSort(Items.OrderBy(keySelector));
        }

        /// <summary>
        /// Сортирует элементы коллекции в порядке убывания по ключу.
        /// </summary>
        /// <typeparam name="TKey"> Тип ключа, возвращаемый <paramref name="keySelector"/>.</typeparam>
        /// <param name="keySelector">Функция для извлечения ключа из элемента.</param>
        public void SortDescending<TKey>(Func<T, TKey> keySelector)
        {
            InternalSort(Items.OrderByDescending(keySelector));
        }

        /// <summary>
        /// Сортирует элементы коллекции в порядке возрастания в соответствии с ключом.
        /// </summary>
        /// <typeparam name="TKey"> Тип ключа, возвращаемый <paramref name="keySelector"/>.</typeparam>
        /// <param name="keySelector">Функция для извлечения ключа из элемента.</param>
        /// <param name="comparer"><see Cref = "IComparer {T}" /> для сравнения ключей.</param>
        public void Sort<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer)
        {
            InternalSort(Items.OrderBy(keySelector, comparer));
        }

        /// <summary>
        /// Перемещает элементы коллекции таким образом, чтобы их порядок совпадал с порядком предоставленных элементов.
        /// </summary>
        /// <param name="sortedItems"> <see cref="IEnumerable{T}"/> для предоставления заказов на товары.</param>
        private void InternalSort(IEnumerable<T> sortedItems)
        {
            var sortedItemsList = sortedItems.ToList();

            foreach (var item in sortedItemsList)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Move(IndexOf(item), sortedItemsList.IndexOf(item));
                },
                _ChildrenEditPrio);

            }
        }
        #endregion // Sorting
    }
}
