namespace SolutionLib.ViewModels.Browser
{
    using InplaceEditBoxLib.Events;
    using SolutionLib.Interfaces;
    using SolutionLib.Models;
    using SolutionLib.ViewModels.Base;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    /// <summary>
    /// Корень решения - это модель просмотра, в которой размещены все другие элементы,
    /// связанные с решением. Даже SolutionRootItem, который является частью отображаемой
    /// коллекции, размещается в коллекции ниже.
    /// </summary>
    internal class SolutionViewModel : ViewModels.Base.BaseViewModel, ISolution
    {
        #region fields
        /// <summary>
        /// Получает расширения файлов, которые можно использовать для загрузки или сохранения
        /// данных древовидного представления в Xml или SQLite.
        /// </summary>
        public readonly string[] fileExtensions = { "solxml", "solsqlite" };

        /// <summary>
        /// Получает удобочитаемое описание для каждого расширения файла, которое
        /// можно использовать для загрузки или сохранения данных древовидного представления в Xml или SQLite.
        /// </summary>
        public readonly string[] fileExtensionDescripts = { "XML Solution files", "SQLite Solution files" };

        private static DispatcherPriority _ChildrenEditPrio = DispatcherPriority.DataBind;

        private ISolutionRootItem _SolutionRootItem = null;
        private readonly ObservableCollection<IItem> _Root = null;
        private ICommand _RenameCommand = null;
        private ICommand _StartRenameCommand;

        private ICommand _SelectionChangedCommand;
        private IItem _SelectedItem;
        private ICommand _ItemAddCommand;
        private ICommand _ItemRemoveCommand;
        private ICommand _ItemRemoveAllCommand;
        #endregion fields

        #region constructors
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SolutionViewModel()
        {
            _Root = new ObservableCollection<IItem>();
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Получает корень древовидной структуры. То есть в 
        /// ObservableCollection есть только 1 элемент, и этот элемент является корневым.
        /// 
        /// Свойство Children этого <see cref = "IItemChildren" />
        /// представляет остальную часть дерева.
        /// </summary>
        public IEnumerable<IItem> Root
        {
            get
            {
                return _Root;
            }
        }

        /// <summary>
        /// Получает фильтр файлов, который применяется, когда пользователь открывает
        /// диалоговое окно сохранения / загрузки для сохранения / загрузки содержимого
        /// древовидного представления решения.
        /// </summary>
        string ISolution.SolutionFileFilter
        {
            get
            {
                string sret = "All Files (*.*) | *.*";

                for (int i = fileExtensions.Length-1; i >= 0; i--)
                {
                     sret = string.Format("{0} (*.{1}) | *.{1}",
                        fileExtensionDescripts[i], fileExtensions[i]) + "|" + sret;
                }

                return sret;

                //// return "((*.solsqllite) | *.solsqllite" +
                ////        "|*.solxml)|*.solxml" +
                ////        "|" + "All Files (*.*)|*.*";
            }
        }

        /// <summary>
        /// Получает фильтр файлов по умолчанию, который применяется, когда пользователь
        /// открывает диалоговое окно сохранения / загрузки для сохранения / загрузки 
        /// содержимого древовидного представления решения в первый раз.
        /// </summary>
        string ISolution.SolutionFileFilterDefault
        {
            get
            {
                return fileExtensions[1];
            }
        }

        #region commands
        /// <summary>
        /// Получает команду, которая добавляет новый элемент в древовидную структуру.
        /// 
        /// Параметр - это кортеж с <see cref = "IItemChildren" />, который является
        /// родительским элементом для создаваемого элемента, и 
        /// <see cref = "SolutionItemType" />, который является типом дочернего
        /// элемента, который должен быть добавлен сюда.
        /// </summary>
        public ICommand ItemAddCommand
        {
            get
            {
                if (_ItemAddCommand == null)
                    _ItemAddCommand = new RelayCommand<object>(p =>
                    {
                        var tuple = p as Tuple<IItemChildren, SolutionItemType>;

                        if (tuple == null)
                            return;

                        var parentItem = tuple.Item1;
                        var addType = tuple.Item2;

                        string nextChildItemName = parentItem.SuggestNextChildName(addType);

                        if (string.IsNullOrEmpty(nextChildItemName) == true)
                            return;

                        IItem item = null;

                        item = parentItem.AddChild(nextChildItemName, addType);
                        parentItem.IsItemExpanded = true;
                        parentItem.SortChildren();

                        if (item != null)
                        {
                            // Запрос EditMode будет работать, только если это будет сделано с НИЗКИМ приоритетом
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)delegate
                            {
                                item.IsItemSelected = true;
                                this.SelectedItem = item;   // Требуется для виртуализированного древовидного представления
                                item.RequestEditMode(InplaceEditBoxLib.Events.RequestEditEvent.StartEditMode);
                            });
                        }
                    },
                    ((p) =>
                    {
                        var tuple = p as Tuple<IItemChildren, SolutionItemType>;

                        if (tuple == null)
                            return false;

                        var parentItem = tuple.Item1;
                        var addType = tuple.Item2;

                        switch (parentItem.ItemType)
                        {
                            // Папка и SolutionRoot должны содержать что угодно
                            case SolutionItemType.Folder:
                            case SolutionItemType.SolutionRootItem:
                                return true;

                            // Файлы не должны иметь собственных дочерних элементов
                            case SolutionItemType.File:
                                return false;

                            // Проекты могут содержать что угодно, кроме проектов.
                            case SolutionItemType.Project:
                                if (addType == SolutionItemType.Project)
                                    return false;
                                else
                                    return true;

                            default:
                                throw new ArgumentOutOfRangeException(parentItem.ItemType.ToString());
                        }
                    }));

                return _ItemAddCommand;
            }
        }

        /// <summary>
        /// Получает команду, удаляющую элемент из древовидной структуры.
        /// </summary>
        public ICommand ItemRemoveCommand
        {
            get
            {
                if (_ItemRemoveCommand == null)
                    _ItemRemoveCommand = new RelayCommand<object>(p =>
                    {
                        var item = p as IItem;

                        if (p == null)
                            return;

                        var parent = item.Parent as IItemChildren;

                        if (parent == null)
                            return;

                        parent.RemoveChild(item);
                    }, (p =>
                    {
                        var item = p as IItem;

                        if (p == null)
                            return false;

                        // Давайте отключим удаление root, поскольку это не так.
                        // кажется, здесь много смысла
                        if (item.Parent is  IItemChildren == false)
                            return false;

                        return true;
                    }));

                return _ItemRemoveCommand;
            }
        }

        /// <summary>
        /// Получает команду, которая удаляет все элементы ниже данного элемента.
        /// </summary>
        public ICommand ItemRemoveAllCommand
        {
            get
            {
                if (_ItemRemoveAllCommand == null)
                {
                    _ItemRemoveAllCommand = new RelayCommand<object>(p =>
                    {
                        var item = p as IItemChildren;

                        if (item == null)
                            return;

                        item.RemoveAllChild();
                    });
                }

                return _ItemRemoveAllCommand;
            }
        }

        /// <summary>
        /// Запускает процесс переименования папки, переименовывая папку,
        /// представленную этой моделью просмотра.
        /// 
        /// Эта команда реализует событие, которое запускает фактический процесс
        /// переименования в подключенном представлении. Подключенное представление,
        /// в свою очередь, вызывает <see cref = "RenameCommand" /> для фактического
        /// переименования данных (если пользователь не отменил тем временем с помощью
        /// клавиши ESC). Итак, переименование на самом деле состоит из 3 частей:
        /// 
        /// 1) ПускПереименование (может быть запущено ею или самим представлением)
        /// 2) Взаимодействие, при котором пользователь взаимодействует с представлением для редактирования строки
        /// 3) RenameCommand -> выполнить переименование в структуре данных и обновить коллекцию элементов
        /// </summary>
        public ICommand StartRenameCommand
        {
            get
            {
                if (_StartRenameCommand == null)
                    _StartRenameCommand = new RelayCommand<object> (it =>
                    {
                        var item = it as IItem;

                        if (item != null)
                        {
                            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
                            {
                                item.RequestEditMode(InplaceEditBoxLib.Events.RequestEditEvent.StartEditMode);
                            });
                         }
                    },
                    (it) =>
                    {
                        var item = it as IItem;

                        if (item != null)
                        {
                            if (item.IsReadOnly == true)
                                return false;
                        }

                        return true;
                    });

                return _StartRenameCommand;
            }
        }

        /// <summary>
        /// Получает команду, которая переименовывает элемент, представленный этой моделью просмотра.
        /// 
        /// Эта команда должна вызываться непосредственно реализующим представлением,
        /// поскольку новое имя элемента доставляется в виде строки с самим элементом 
        /// в качестве второго параметра через привязку через свойство зависимостей
        /// RenameCommandParameter.
        /// </summary>
        public ICommand RenameCommand
        {
            get
            {
                if (_RenameCommand == null)
                {
                    _RenameCommand = new RelayCommand<object>((p) =>
                    {
                        var tuple = p as Tuple<string, object>;

                        if (tuple != null)
                        {
                            var solutionItem = tuple.Item2 as IItem;

                            if (tuple.Item1 != null && solutionItem != null)
                            {
                                string newName = tuple.Item1;

                                // Мы уже знаем этот предмет?
                                if (string.IsNullOrEmpty(newName) == true ||
                                  newName.Length < 1 || newName.Length > 254)
                                {
                                    solutionItem.RequestEditMode(RequestEditEvent.StartEditMode);
                                    solutionItem.ShowNotification("Invalid legth of name",
                                        "A name must be between 1 and 254 characters long.");
                                    return;
                                }

                                var parent = solutionItem.Parent as IItemChildren;

                                if (parent != null)
                                {
                                    //Мы уже знаем этот предмет?
                                    var existingItem = parent.FindChild(newName);
                                    if (existingItem != null && existingItem != solutionItem)
                                    {
                                        solutionItem.RequestEditMode(RequestEditEvent.StartEditMode);
                                        solutionItem.ShowNotification("Item Already Exists",
                                            "An item with this name exists already. All names must be unique.");

                                        return;
                                    }

                                    parent.RenameChild(solutionItem, newName);


                                    //Этот родительский выбор + сортировка + дочерний
                                    //выбор прокручивает переименованный элемент в поле зрения ...
                                    parent.IsItemSelected = true;
                                    parent.IsItemExpanded = true;   //Убедитесь, что родительский элемент расширен
                                    parent.SortChildren();
                                    solutionItem.IsItemSelected = true;
                                }
                                else
                                {
                                    // Это корневой элемент - он может затем переименовать себя
                                    var solutionRootItem = tuple.Item2 as ISolutionRootItem;
                                    newName = tuple.Item1;

                                    if (solutionRootItem != null &&
                                        string.IsNullOrEmpty( newName ) == false)
                                    {
                                        solutionRootItem.RenameRootItem(newName);
                                    }
                                }
                            }
                        }
                    });
                }

                return _RenameCommand;
            }
        }

        public ICommand SelectionChangedCommand
        {
            get
            {
                if (_SelectionChangedCommand == null)
                {
                    _SelectionChangedCommand = new RelayCommand<object>((p) =>
                    {
                        var para = p as IItem;
                        SelectedItem = para;
                    });
                }

                return _SelectionChangedCommand;
            }
        }

        /// <summary>
        /// Получает текущий выбранный элемент из коллекции элементов дерева.
        /// </summary>
        public IItem SelectedItem
        {
            get { return _SelectedItem; }

            set
            {
                if (_SelectedItem != value)
                {
                    _SelectedItem = value;
                    NotifyPropertyChanged(() => SelectedItem);
                }
            }     
        }
        #endregion commands
        #endregion properties

        #region methods
        /// <summary>
        /// Сбрасывает все элементы модели просмотра в исходное состояние времени построения.
        /// </summary>
        public void ResetToDefaults()
        {
            AddSolutionRootItem("New Solution");
        }


        /// <summary>
        /// Переименовывает отображаемую строку в <paramref name = "solutionItem" />
        /// в соответствии с запросом в <paramref name = "newDisplayName" />.
        /// </summary>
        /// <param name="solutionItem"></param>
        /// <param name="newDisplayName"></param>
        public void RenameItem(IItem solutionItem, string newDisplayName)
        {
            SelectedItem = null;
            //solutionItem.SetDisplayName(newDisplayName);
        }

        /// <summary>
        /// Добавляет корень решения в коллекцию элементов решения.
        /// 
        /// Будьте осторожны (!), Поскольку текущий корневой элемент (если есть)
        /// отбрасывается вместе со всеми его дочерними элементами, поскольку модель
        /// просмотра всегда поддерживает только ОДИН корень.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public IItemChildren AddSolutionRootItem(string displayName)
        {
            if (_SolutionRootItem != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _Root.Remove(_SolutionRootItem);
                },
                _ChildrenEditPrio);
                
                _SolutionRootItem = null;
            }

            var rootItem = new SolutionRootItemViewModel(null, displayName, false);

            _SolutionRootItem = rootItem;
            Application.Current.Dispatcher.Invoke(() =>
            {
                _Root.Add(_SolutionRootItem);
            },
            _ChildrenEditPrio);

            return _SolutionRootItem;
        }

        /// <summary>
        /// Добавляет еще один дочерний элемент ниже корневого элемента
        /// в коллекцию. Это вызовет исключение, если родительский элемент
        /// равен нулю.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public IItem AddRootChild(
            string itemName,
            SolutionItemType itemType)
        {
            if (_SolutionRootItem == null)
                throw new System.Exception("Solution Root Item must be created BEFORE adding children!");

            return AddChild(itemName, itemType, _SolutionRootItem);
        }

        /// <summary>
        /// Добавляет еще один файловый (дочерний) элемент под
        /// родительским элементом. Это вызовет исключение, если
        /// родительский элемент равен нулю.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="parent"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public IItem AddChild(string itemName,
                                  SolutionItemType itemType,
                                  IItemChildren parent)
        {
            if (parent == null)
                throw new System.ArgumentException("Paremeter parent cannot have children.");

            switch (itemType)
            {
                case SolutionItemType.SolutionRootItem:
                    return AddSolutionRootItem(itemName);

                case SolutionItemType.File:
                    return parent.AddFile(itemName);

                case SolutionItemType.Folder:
                    return parent.AddFolder(itemName);

                case SolutionItemType.Project:
                    return parent.AddProject(itemName);

                default:
                    throw new ArgumentOutOfRangeException(itemType.ToString());
            }
        }

        /// <summary>
        /// Возвращает первый видимый элемент в древовидной структуре (если есть) или null.
        /// Этот метод представляет собой удобную оболочку, которая разворачивает свойство
        /// <see cref = "Root" />, поскольку модель просмотра всегда поддерживает только ОДИН корень.
        /// </summary>
        /// <returns></returns>
        public IItemChildren GetRootItem()
        {
            if (_Root.Count == 0)
                return null;

            return _Root.First() as IItemChildren;
        }
        #endregion methods
    }
}
