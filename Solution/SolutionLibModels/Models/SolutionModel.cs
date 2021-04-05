namespace SolutionModelsLib.Models
{
    using SolutionModelsLib.Enums;
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models.Base;
    using System;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// SolutionModel - это класс, в котором размещаются все остальные элементы, связанные
    /// с решением. Даже SolutionRootItem, который является частью отображаемой коллекции, 
    /// размещается в коллекции ниже.
    /// </summary>
    internal class SolutionModel : ISolutionModel
    {
        #region constructors
        /// <summary>
        /// Paremeterless standard constructor.
        /// </summary>
        public SolutionModel()
        {
            Version = 1;
            MinorVersion = 0;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets the version of this model.
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// Gets the minor version of this model.
        /// </summary>
        public int MinorVersion { get; private set; }

        /// <summary>
        /// Получает корень древовидной структуры. То есть в ObservableCollection 
        /// есть только 1 элемент, и этот элемент является корневым.
        /// 
        /// Свойство Children
        /// этого <see cref="SolutionRootItemModel"/> представляет остальную часть дерева.
        /// </summary>
        public ISolutionRootItemModel Root { get; set; }
        #endregion properties

        #region methods

        /// <summary>
        /// Добавляет дочерний элемент с заданным типом
        /// (<see cref="SolutionItemType.SolutionRootItem"/> не может быть добавлен).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IItemModel AddStaticChild(string displayName
                                 , SolutionModelItemType type
                                 , IItemChildrenModel parent)
        {
            if (parent.FindChild(displayName) != null)
                throw new ArgumentException("Item '" + displayName + "' already exists.");

            IItemModel newItem = null;

            switch (type)
            {
                case SolutionModelItemType.File:
                    newItem = new FileItemModel(parent, displayName);
                    break;
                case SolutionModelItemType.Folder:
                    newItem = new FolderItemModel(parent, displayName);
                    break;
                case SolutionModelItemType.Project:
                    newItem = new ProjectItemModel(parent, displayName);
                    break;

                // This should be created via AddSolutionRootItem() method
                case SolutionModelItemType.SolutionRootItem:
                default:
                    throw new ArgumentException(type.ToString());
            }

            parent.AddChild(newItem);

            return newItem;
        }

        /// <summary>
        /// Creates a new solution root item from the given parameters
        /// (replacing the current root item if there is any)
        /// and returns its interface.
        /// Создает новый корневой элемент решения из заданных параметров
        /// (заменяет текущий корневой элемент, если он есть) и возвращает его интерфейс.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IItemChildrenModel AddSolutionRootItem(string displayName, long id = -1)
        {
            Root = new SolutionRootItemModel(displayName) { Id = id };

            return Root;
        }

        /// <summary>
        /// Добавляет корневой элемент решения (заменяет текущий корневой элемент, если он есть)
        /// и возвращает его интерфейс.
        /// </summary>
        /// <param name="rootItem"></param>
        /// <returns></returns>
        public IItemChildrenModel AddSolutionRootItem(ISolutionRootItemModel rootItem)
        {
            Root = rootItem;

            return Root;
        }

        /// <summary>
        /// Добавляет дочерний элемент с заданным типом
        /// (<see cref="SolutionItemType.SolutionRootItem"/> здесь нельзя добавить).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IItemModel AddChild(string displayName
                                 , SolutionModelItemType type
                                 , IItemChildrenModel parent)
        {
            return AddItem(displayName, type, parent);
        }

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionItemType.SolutionRootItem"/> cannot be added here).
        /// 
        /// This wrapper uses a long input for conversion when reading from file.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IItemModel AddChild(string displayName
                                   , long longType
                                   , IItemChildrenModel parent)
        {
            if (parent.FindChild(displayName) != null)
                throw new ArgumentException("Item '" + displayName + "' already exists.");

            SolutionModelItemType type = (SolutionModelItemType)longType;

            return AddChild(displayName, type, parent);
        }

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionItemType.SolutionRootItem"/> cannot be added here).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static IItemModel AddItem(string displayName
                                         , SolutionModelItemType type
                                         , IItemChildrenModel parent)
        {
            if (parent.FindChild(displayName) != null)
                throw new ArgumentException("Item '" + displayName + "' already exists.");

            IItemModel newItem = null;

            switch (type)
            {
                case SolutionModelItemType.File:
                    newItem = new FileItemModel(parent, displayName);
                    break;
                case SolutionModelItemType.Folder:
                    newItem = new FolderItemModel(parent, displayName);
                    break;
                case SolutionModelItemType.Project:
                    newItem = new ProjectItemModel(parent, displayName);
                    break;

                // This should be created via AddSolutionRootItem() method
                case SolutionModelItemType.SolutionRootItem:
                default:
                    throw new ArgumentException(type.ToString());
            }

            parent.AddChild(newItem);

            return newItem;
        }

        #region IXmlSerializable interface
        /// <summary>
        /// Implements the GetSchema() method of the <seealso cref="IXmlSerializable"/> interface
        /// and returns null (or Nothing in Visual Basic).
        /// </summary>
        /// <returns></returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Implements the ReadXml() method of the <seealso cref="IXmlSerializable"/> interface.
        /// </summary>
        /// <param name="reader"></param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            int i = 0;

            int.TryParse(reader.GetAttribute("Version") , out i);
            Version = i;

            int.TryParse(reader.GetAttribute("MinorVersion"), out i);
            MinorVersion = i;

            reader.ReadStartElement();  // Consume SolutionModel Tag

            reader.MoveToContent();

            // Invoke ReadXml in SolutionRootItemModel to finish this off
            var rootItem = ReadItem(SolutionModelItemType.SolutionRootItem, reader) as ISolutionRootItemModel;
            this.AddSolutionRootItem(rootItem);
        }

        /// <summary>
        /// Implements the WriteXml() method of the <seealso cref="IXmlSerializable"/> interface.
        /// </summary>
        /// <param name="writer"></param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Version", Version.ToString());
            writer.WriteAttributeString("MinorVersion", MinorVersion.ToString());

            // RootItems are written here...
            if (Root != null)
            {
                var rootSer = new DataContractSerializer(typeof(SolutionRootItemModel));
                rootSer.WriteObject(writer, Root);
            }
        }
        #endregion IXmlSerializable interface

        /// <summary>
        /// Reads the indicated item <paramref name="type"/> from the Xml reader's
        /// data stream, advances the Xml reader, and returns the new item.
        /// Считывает указанный элемент <paramref name="type"/> из потока данных 
        /// средства чтения Xml, продвигает средство чтения Xml и возвращает новый элемент.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static IItemModel ReadItem(SolutionModelItemType type
                                          , XmlReader reader)
        {
            IItemModel newItem = null;
            DataContractSerializer itemSer = null;

            switch (type)
            {
                case SolutionModelItemType.File:
                    itemSer = new DataContractSerializer(typeof(FileItemModel));
                    newItem = (FileItemModel)itemSer.ReadObject(reader);
                    break;

                case SolutionModelItemType.Folder:
                    itemSer = new DataContractSerializer(typeof(FolderItemModel));
                    newItem = (FolderItemModel)itemSer.ReadObject(reader);
                    break;

                case SolutionModelItemType.Project:
                    itemSer = new DataContractSerializer(typeof(ProjectItemModel));
                    newItem = (ProjectItemModel)itemSer.ReadObject(reader);
                    break;

                case SolutionModelItemType.SolutionRootItem:
                    itemSer = new DataContractSerializer(typeof(SolutionRootItemModel));
                    newItem = (SolutionRootItemModel)itemSer.ReadObject(reader);
                    break;

                default:
                    throw new ArgumentException(type.ToString());
            }

            return newItem;
        }

        /// <summary>
        /// Создает зависящий от типа объект сериализатора, который соответствует
        /// заданному <paramref name="item"/>, и записывает его Xml-содержимое в 
        /// Xml <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="item"></param>
        internal static void SerializeItem(XmlWriter writer, IItemModel item)
        {
            DataContractSerializer itemSer = null;
            switch (item.ItemType)
            {
                case SolutionModelItemType.SolutionRootItem:
                    itemSer = new DataContractSerializer(typeof(SolutionRootItemModel));
                    break;
                case SolutionModelItemType.File:
                    itemSer = new DataContractSerializer(typeof(FileItemModel));
                    break;
                case SolutionModelItemType.Folder:
                    itemSer = new DataContractSerializer(typeof(FolderItemModel));
                    break;
                case SolutionModelItemType.Project:
                    itemSer = new DataContractSerializer(typeof(ProjectItemModel));
                    break;
                default:
                    throw new System.NotImplementedException(item.ItemType.ToString());
            }

            itemSer.WriteObject(writer, item);
        }

        /// <summary>
        /// Получает имя соответствующего XML для каждого типа модели на 
        /// основе перечислимого значения.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string GetXmlName(SolutionModelItemType type)
        {
            switch (type)
            {
                case SolutionModelItemType.SolutionRootItem:
                    return "RootItem";
                case SolutionModelItemType.File:
                    return "File";
                case SolutionModelItemType.Folder:
                    return "Folder";
                case SolutionModelItemType.Project:
                    return "Project";
                default:
                    throw new System.NotImplementedException(type.ToString());
            }
        }

        /// <summary>
        /// Коллекция элементов может содержать любой элемент, производный от <see cref="ItemModel"/>.
        /// Таким образом, задача этого метода - определить правильный класс и заставить работать его 
        /// метод <see cref="IXmlSerializable.ReadXml(XmlReader)"/>.
        /// 
        /// Background Info on ReadXml():
        /// https://docs.microsoft.com/de-de/dotnet/api/system.xml.serialization.ixmlserializable.readxml?view=netframework-4.5.2#System_Xml_Serialization_IXmlSerializable_ReadXml_System_Xml_XmlReader_
        /// 
        /// При вызове этого метода средство чтения размещается на начальном теге, который содержит
        /// информацию для вашего типа. То есть непосредственно на начальном теге, который указывает
        /// начало сериализованного объекта. Когда этот метод возвращается, он должен прочитать весь
        /// элемент от начала до конца, включая все его содержимое. В отличие от метода WriteXml,
        /// платформа не обрабатывает элемент оболочки автоматически. Ваша реализация должна это сделать.
        /// Несоблюдение этих правил позиционирования может привести к тому, что код будет генерировать 
        /// неожиданные исключения времени выполнения или повреждать данные.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="parent"></param>
        internal static void ReadItemsCollection(XmlReader reader,
                                                 ItemChildrenModel parent)
        {
            try
            {
                SolutionModelItemType[] childType =  // Элементы, которые могут встречаться в коллекции Items
                {
                    SolutionModelItemType.Project
                    ,SolutionModelItemType.Folder
                    ,SolutionModelItemType.File
                };

                while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                    reader.Read();

                reader.ReadStartElement("Items");

                reader.MoveToContent();
                while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                    reader.Read();

                if (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                    {
                        bool bProcessedElement = false;

                        // Reading a Project, Folder, or File item and adding it to the collection
                        foreach (var item in childType)
                        {
                            if (SolutionModel.GetXmlName(item) == reader.LocalName)
                            {
                                IItemModel newItem = SolutionModel.ReadItem(item, reader);

                                newItem.Parent = parent;
                                parent.AddChild(newItem);

                                bProcessedElement = true;
                                break;
                            }
                        }

                        if (bProcessedElement == false)
                            throw new System.NotSupportedException(reader.LocalName);

                        while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                            reader.Read();
                    }

                    reader.ReadEndElement();
                }

                while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                    reader.Read();

                reader.ReadEndElement();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion methods
    }
}
