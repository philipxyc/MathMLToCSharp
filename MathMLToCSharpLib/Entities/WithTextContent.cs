using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace MathMLToCSharpLib.Entities
{
    public abstract class WithTextContent : IBuildable
    {
        public string content;
        protected WithTextContent() { }
        protected WithTextContent(string content)
        {
            this.content = content;
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }


        #region IBuildable Members

        public virtual void Visit(StringBuilder sb, BuildContext bc)
        {
            bc.Tokens.Add(this);
            sb.Append(content);
        }

        /*public XElement ToXElement()
        {
            return new XElement(this.GetType().Name, content);
        }*/

        #region IXmlSerializable Members
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.HasValue)
                {
                    content = reader.Value;
                    return;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(this.GetType().Name);
            writer.WriteString(content);
            writer.WriteEndElement();
        }
        #endregion

        #endregion


    }
}