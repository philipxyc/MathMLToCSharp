using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace MathMLToCSharpLib.Entities
{
    public abstract class WithBuildableContent : IBuildable
    {
        protected IBuildable content;

        protected WithBuildableContent() { }
        protected WithBuildableContent(IBuildable content)
        {
            this.content = content;
        }

        #region IBuildable Members

        public virtual void Visit(StringBuilder sb, BuildContext bc)
        {
            bc.Tokens.Add(this);
            content.Visit(sb, bc);
        }

        /*public XElement ToXElement()
        {
            return new XElement(this.GetType().Name, content.ToXElement());
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
                if (reader.IsStartElement())
                {
                    Type type = Type.GetType(this.GetType().Namespace + "." + reader.Name);
                    content = (IBuildable)Activator.CreateInstance(type);
                    content.ReadXml(reader);
                }
                else if (reader.NodeType == XmlNodeType.EndElement & reader.Name == this.GetType().Name)
                {
                    return;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(this.GetType().Name);
            content.WriteXml(writer);
            writer.WriteEndElement();
        }
        #endregion

        #endregion
    }
}