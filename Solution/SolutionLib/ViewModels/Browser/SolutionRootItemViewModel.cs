namespace SolutionLib.ViewModels.Browser
{
    using SolutionLib.Interfaces;

    /// <summary>
    /// Реализует класс модели просмотра для первого видимого элемента в древовидной структуре.
    /// Обычно в любом дереве есть только один корень, поэтому этот класс реализует этот один
    /// элемент, визуально представляющий этот корень (например: элемент «Компьютер» в проводнике Windows).
    /// </summary>
    internal class SolutionRootItemViewModel : Base.ItemChildrenViewModel, ISolutionRootItem
    {
        #region constructors
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="parent"></param>
        /// <param name="addDummyChild"></param>
        public SolutionRootItemViewModel(IItem parent
                                       , string displayName
                                       , bool addDummyChild = true)
            : base(parent, Models.SolutionItemType.SolutionRootItem, addDummyChild)
        {
            SetDisplayName(displayName);
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        protected SolutionRootItemViewModel()
            : base(null, Models.SolutionItemType.SolutionRootItem)
        {
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// Переименуйте элемент отображения корневого элемента.
        /// </summary>
        /// <param name="newName"></param>
        public void RenameRootItem(string newName)
        {
            SetDisplayName(newName);
        }
        #endregion methods
        }
    }
