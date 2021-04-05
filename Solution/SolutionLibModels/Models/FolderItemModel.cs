namespace SolutionModelsLib.Models
{
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models.Base;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Реализует интерфейс к модели представления элемента папки в
    ///коллекцию элементов с древовидной структурой.
    ///Типы коллекционных предметов можно различить по:
    ///1) интерфейс
    ///(например, чтобы выбрать шаблон в ItemTemplateSelector или
    ///для использования в HierarchicalDataTemplate,
    ///или же
    ///2) путем перечисления в <see cref="SolutionLib.Models.SolutionItemType"/>.
    /// </summary>
    [XmlRoot("Folder")]
    internal class FolderItemModel : ItemChildrenModel, IFolderItemModel
    {
        #region constructors
        /// <summary>
        /// Параметризованный конструктор для обычного использования, когда новые 
        /// элементы создаются через другие модели просмотра через пользовательский интерфейс.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="displayName"></param>
        public FolderItemModel(IItemModel parent, string displayName)
            : base(parent, displayName, Enums.SolutionModelItemType.Folder)
        {
        }

        /// <summary>
        /// Конструктор по умолчанию без параметров, необходимый для десериализации XML.
        /// </summary>
        internal FolderItemModel()
            : base(null, string.Empty, Enums.SolutionModelItemType.Folder)
        {

        }
        #endregion constructors

        #region methods
        #region IXmlSerializable methods
        /// <summary>
        /// Реализует метод ReadXml () интерфейса <seealso cref="IXmlSerializable"/>.
        /// </summary>
        /// <param name="reader"></param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            try
            {
                while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                    reader.Read();

                DisplayName = reader.GetAttribute("name");

                long idValue = -1;
                long.TryParse(reader.GetAttribute("id"), out idValue);
                this.Id = idValue;

                reader.ReadStartElement();  // Consum Folder Tag

                reader.MoveToContent();
                while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                    reader.Read();

                // Read Items collection and items below it
                if (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                    SolutionModel.ReadItemsCollection(reader, this);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Реализует метод WriteXml () интерфейса <seealso cref="IXmlSerializable"/>.
        /// </summary>
        /// <param name="writer"></param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("name", this.DisplayName);
            writer.WriteAttributeString("id", this.Id.ToString());

            // Child Items are written here...
            writer.WriteStartElement("Items");
            foreach (var item in Children)
            {
                SolutionModel.SerializeItem(writer, item);
            }
            writer.WriteEndElement();
        }
        #endregion IXmlSerializable methods
        #endregion methods
    }
}
