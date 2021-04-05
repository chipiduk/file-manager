namespace SolutionLib.ViewModels.Browser.Base
{
    using SolutionLib.Interfaces;
    using SolutionLib.Models;
    using SolutionLib.ViewModels.Collections;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Реализует базовую функциональность для всех элементов, которые, в свою
    /// очередь, могут иметь коллекции <see cref = "Children" /> (обычно
    /// привязанные к ItemSource в Treeview или HierarchicalDataTemplate).
    /// </summary>
    internal class ItemChildrenViewModel  : ItemViewModel, IItemChildren
    {
        #region fields
        private static readonly ItemChildrenViewModel DummyChild = new ItemChildrenViewModel();

        private readonly SortableObservableDictionaryCollection _Children;
        #endregion fields

        #region constructors
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="itemType"></param>
        /// <param name="addDummyChild"></param>
        protected ItemChildrenViewModel(IItem parent
                                      , SolutionItemType itemType
                                      , bool addDummyChild = true)
            : base(parent, itemType)
        {
            _Children = new SortableObservableDictionaryCollection();

            ResetChildren(addDummyChild); // Позволяет лениво загружать дочерние элементы
        }

        /// <summary>
        /// Конструктор скрытого класса может использоваться только для
        /// создания экземпляра статического элемента <see cref = "DummyChild" />.
        /// </summary>
        private ItemChildrenViewModel()
            : base()
        {
            _Children = new SortableObservableDictionaryCollection();

            //Не делайте этого с true, так как это 
            //добавит фиктивного ребенка ниже фиктивного
            //ребенка и так далее ...
            ResetChildren(false); // Не будем лениво загружать дочерние элементы в этот ctor
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Получает отсортированную коллекцию дочерних элементов, которые могут
        /// быть извлеченным из этого родительского элемента.
        /// </summary>
        public IEnumerable<IItem> Children
        {
            get
            {
                return _Children;
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Находит дочерний элемент по заданному ключу или возвращает null.
        /// </summary>
        /// <param name="displyName"></param>
        /// <returns></returns>
        public IItem FindChild(string displyName)
        {
            return _Children.TryGet(displyName);
        }

        /// <summary>
        /// Добавление нового следующего дочернего элемента через поле редактирования
        /// на месте требует, чтобы мы знали, является ли «Новая папка», «Новая папка 1»,
        /// «Новая папка 2» ... следующим подходящим именем - этот метод определяет это
        /// имя и возвращает его для данного типа (создаваемого) дочернего элемента.
        /// </summary>
        /// <param name="nextTypeTpAdd"></param>
        /// <returns></returns>
        public string SuggestNextChildName(SolutionItemType nextTypeTpAdd)
        {
            string suggestMask = null;

            switch (nextTypeTpAdd)
            {
                case SolutionItemType.SolutionRootItem:
                    suggestMask = "New Solution";
                    break;
                case SolutionItemType.File:
                    suggestMask = "New File";
                    break;
                case SolutionItemType.Folder:
                    suggestMask = "New Folder";
                    break;
                case SolutionItemType.Project:
                    suggestMask = "New Project";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nextTypeTpAdd.ToString());
            }

            var nextChild = _Children.TryGet(suggestMask);
            if (nextChild == null)
                return suggestMask;

            string suggestChild = null;
            for (int i = 1; i < _Children.Count + 100; i++)
            {
                suggestChild = string.Format("{0} {1}", suggestMask, i);

                nextChild = _Children.TryGet(suggestChild);
                if (nextChild == null)
                    break;
            }

            return suggestChild;
        }

        /// <summary>
        /// Добавляет дочерний элемент типа <see cref="IItem"/> к этому
        /// родительскому элементу, который также можно ввести с помощью <see cref="IItem"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IItem AddChild(IItem item)
        {
            return AddChild(item.DisplayName, item);
        }

        /// <summary>
        /// Добавляет дочерний элемент с заданным типом 
        /// (здесь нельзя добавить <see cref = "SolutionItemType.SolutionRootItem" />).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IItem AddChild(string displayName, SolutionItemType type)
        {
            if (HasDummyChild == true)
                ResetChildren(false);

            switch (type)
            {
                case SolutionItemType.File:
                    return AddChild(displayName, new FileViewModel(this, displayName));

                case SolutionItemType.Folder:
                    return AddChild(displayName, new FolderViewModel(this, displayName, false));

                case SolutionItemType.Project:
                    return AddChild(displayName, new ProjectViewModel(this, displayName, false));

                default:
                case SolutionItemType.SolutionRootItem:
                    // это создается только в SolutionViewModel
                    throw new System.ArgumentOutOfRangeException(type.ToString());
            }
        }

        /// <summary>
        /// Удаляет дочерний элемент из коллекции дочерних элементов этого элемента.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveChild(IItem item)
        {
            if (item == null)
                return false;

            item.SetParent(null);

            var itemIsSelected = item.IsItemSelected;
            var idx = _Children.IndexOf(item);
            var removedItem = _Children.RemoveItem(item);

            if (itemIsSelected == false)
                return removedItem;

            // Удаленный элемент был выбран, поэтому давайте попробуем выбрать что-нибудь поблизости
            if (idx <= 0)
                this.IsItemSelected = true;
            else
                _Children[idx - 1].IsItemSelected = true;

            return removedItem;
        }

        /// <summary>
        /// Переименовывает дочерний элемент в коллекции дочерних элементов этого элемента.
        /// После переименования следует применить повторную сортировку и IsItemSelected,
        /// чтобы переименованный элемент снова появился в правильной позиции в отсортированном списке элементов.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public void RenameChild(IItem item, string newName)
        {
            if (item == null)
                return;

            _Children.RenameItem(item, newName);
        }

        /// <summary>
        /// Удаляет всех дочерних элементов (если есть) ниже этого элемента.
        /// </summary>
        public void RemoveAllChild()
        {
            ResetChildren(false);
        }

        /// <summary>
        /// Сортирует все элементы для отображения в отсортированном виде.
        /// </summary>
        public void SortChildren()
        {
            _Children.SortItems();
        }

        /// <summary>
        /// Добавляет другой элемент папки (дочерний) в данную коллекцию элементов.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IItem IItemChildren.AddFolder(string displayName)
        {
            return AddChild(displayName, SolutionItemType.Folder);
        }

        /// <summary>
        /// Добавляет другой проект (дочерний) элемент в данную коллекцию элементов.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IItem IItemChildren.AddProject(string displayName)
        {
            return AddChild(displayName, SolutionItemType.Project);
        }

        /// <summary>
        /// Добавляет другой файловый (дочерний) элемент в данную коллекцию элементов.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IItem IItemChildren.AddFile(string displayName)
        {
            return AddChild(displayName, SolutionItemType.File);
        }

        protected virtual bool HasDummyChild
        {
            get
            {
                if (this._Children.Count == 1)
                {
                    if (this._Children[0] == DummyChild)
                        return true;
                }

                return false;
            }
        }

        protected virtual void ResetChildren(bool addDummyChild = true)
        {
            _Children.Clear();

            if (addDummyChild == true)
                _Children.Add(DummyChild);
        }

        /// <summary>
        /// Добавляет дочерний элемент типа <see cref = "IItem" /> к этому
        /// родительскому элементу, который также можно ввести с помощью <see cref = "IItem" />.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected IItem AddChild(string key, IItem value)
        {
            try
            {
                _Children.AddItem(value);
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Failed to add item key:{0} - '{1}' below {2} - '{3}'"
                    , key, value, DisplayName, this));
            }

            return value;
        }
        #endregion methods
    }
}
