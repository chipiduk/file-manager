namespace SolutionModelsLib.Models
{
    using System.Xml;
    using System.Xml.Serialization;
    using SolutionModelsLib.Enums;
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models.Base;

    /// <summary>
    /// Реализует интерфейс для класса модели первого видимого элемента в древовидной структуре.
    /// 
    /// Обычно в любом дереве есть только один корень, поэтому этот класс реализует этот один 
    /// элемент, визуально представляющий этот корень (например: элемент «Компьютер» в проводнике Windows).
    /// </summary>
    [XmlRoot("RootItem")]
    internal class SolutionRootItemModel : ItemChildrenModel, ISolutionRootItemModel
    {
        #region constructors
        /// <summary>
        /// Параметризованный конструктор для обычного использования, когда новые элементы
        /// создаются через другие модели просмотра через пользовательский интерфейс.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="parent"></param>
        public SolutionRootItemModel(string displayName)
            : base(Enums.SolutionModelItemType.SolutionRootItem)
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// Конструктор по умолчанию без параметров, необходимый для десериализации XML.
        /// </summary>
        protected SolutionRootItemModel()
            : base(Enums.SolutionModelItemType.SolutionRootItem)
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

                this.DisplayName = reader.GetAttribute("name");

                long idValue = -1;
                long.TryParse(reader.GetAttribute("id"), out idValue);
                this.Id = idValue;

                reader.ReadStartElement();  // Consum RootItem Tag

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
