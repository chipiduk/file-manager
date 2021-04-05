namespace SolutionLib.ViewModels.Browser
{
    using SolutionLib.Interfaces;

    internal class FolderViewModel : Base.ItemChildrenViewModel, IFolder
    {
        #region constructors
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="displayName"></param>
        /// <param name="addDummyChild"></param>
        public FolderViewModel(IItem parent
                             , string displayName
                             , bool addDummyChild = true)
           : base(parent, Models.SolutionItemType.Folder, addDummyChild)
        {
            SetDisplayName(displayName);
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        protected FolderViewModel()
           : base(null, Models.SolutionItemType.Folder)
        {
        }
        #endregion constructors
    }
}
