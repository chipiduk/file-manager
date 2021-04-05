namespace SolutionLib.ViewModels.Collections
{
    using SolutionLib.Interfaces;
    using SolutionLib.Models;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Реализует настраиваемую наблюдаемую коллекцию, в которой могут размещаться 
    /// элементы, набранные с помощью <see cref="SolutionItemType"/> - эти элементы
    /// сортируются и сохраняются уникальными для повторного преобразования структуры,
    /// аналогичной структуре обозревателя решений Visual Studio.
    /// </summary>
    internal class SortableObservableDictionaryCollection : SortableObservableCollection<IItem>
    {
        #region fields
        private Dictionary<string, IItem> _dictionary = null;
        private static DispatcherPriority _ChildrenEditPrio = DispatcherPriority.DataBind;
        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor.
        /// </summary>
        public SortableObservableDictionaryCollection()
        {
            _dictionary = new Dictionary<string, IItem>();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// Добавляет новый элемент в коллекцию размещенных элементов.
        /// Метод выдает исключение, если ключ нового элемента уже
        /// присутствует в текущей коллекции.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(IItem item)
        {
            item.SortKey = GenSortKey(item);
            _dictionary.Add(item.DisplayName, item);

            Application.Current.Dispatcher.Invoke(() => { base.Add(item); }, _ChildrenEditPrio); //я так понял, что создается элемент в интерфейсе (обновление интерфейса после добавления нового элемента)

            return true;
        }

        /// <summary>
        /// Добавляет новый элемент в коллекцию размещенных элементов.
        /// Метод выдает исключение, если ключ нового элемента уже
        /// присутствует в текущей коллекции.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new bool Add(IItem item)
        {
            return AddItem(item);
        }

        /// <summary>
        /// Удаляет элемент из коллекции размещенных элементов.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveItem(IItem item)
        {
            item.SortKey = GenSortKey(item);

            _dictionary.Remove(item.DisplayName);
            
            Application.Current.Dispatcher.Invoke(() => { base.Remove(item); }, _ChildrenEditPrio);

            return true;
        }

        /// <summary>
        /// Удаляет элемент из коллекции размещенных элементов.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new bool Remove(IItem item)
        {
            return RemoveItem(item);
        }

        /// <summary>
        /// Удаляет все элементы в коллекции.
        /// </summary>
        public new void Clear()
        {
            _dictionary.Clear();
            Application.Current.Dispatcher.Invoke(() => { base.Clear(); }, _ChildrenEditPrio);
        }

        /// <summary>
        /// Пытается найти элемент во внутреннем словаре и возвращает его или
        /// возвращает null, если элемент был недоступен.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IItem TryGet(string key)
        {
            IItem o;

            if (_dictionary.TryGetValue(key, out o))
                return o;

            return null;
        }

        /// <summary>
        /// Переименовывает элемент в коллекции, а также настраивает его ключ
        /// сортировки, чтобы убедиться, что переименованный элемент снова появляется на своем месте.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newName"></param>
        public void RenameItem(IItem item, string newName)
        {
            _dictionary.Remove(item.DisplayName);
            item.SetDisplayName(newName);
            
            item.SortKey = GenSortKey(item);
            _dictionary.Add(newName, item);
        }

        /// <summary>
        /// Сортирует элементы в этой коллекции по запросу.
        /// </summary>
        public void SortItems()
        {
            this.Sort(item => item.SortKey);
        }

        /// <summary>
        /// Метод сгенерирует ключ сортировки, подходящий для сортировки (не для уникальной идентификации элемента)
        /// Сортирует позиции папок, файлов и проекта (что перед чем идет).
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GenSortKey(IItem item)
        {
            string key = item.DisplayName;
            SolutionItemType itemType = item.ItemType;

            // Вычислите префикс для создания группы, порядок сортировки для отображения элементов в:
            // Проекты (группы, отсортированные по алфавиту)
            // Папки (группы, отсортированные по алфавиту)
            // Файлы (группы, отсортированные по алфавиту)
            string prefix = "";
            switch (itemType)
            {
                case SolutionItemType.SolutionRootItem:
                default:
                    prefix = "000_";
                    break;

                case SolutionItemType.Folder:
                    prefix = "222_";
                    break;

                case SolutionItemType.Project:
                    prefix = "444_";
                    break;

                case SolutionItemType.File:
                    prefix = "666_";
                    break;

            }

            return prefix + key;
        }
        #endregion methods
    }
}
