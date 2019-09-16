using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace MathMLToCSharpLib.Entities
{
    public abstract class WithBinaryContent : IBuildable
    {
        public IBuildable first, second;

        protected WithBinaryContent() { }
        protected WithBinaryContent(IBuildable first, IBuildable second)
        {
            this.first = first;
            this.second = second;
        }

        public Tuple<IBuildable, IBuildable> Contents
        {
            get
            {
                return new Tuple<IBuildable, IBuildable>(first, second);
            }
        }

        #region IBuildable Members

        public abstract void Visit(StringBuilder sb, BuildContext bc);

        /*public XElement ToXElement()
        {
            return new XElement(this.GetType().Name, first.ToXElement(), second.ToXElement());
        }*/

        #region IXmlSerializable Members
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            int i = 0;
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    Type type = Type.GetType(this.GetType().Namespace + "." + reader.Name);
                    IBuildable element = (IBuildable)Activator.CreateInstance(type);
                    element.ReadXml(reader);

                    if (i == 0)
                        first = element;
                    else
                        second = element;
                    i++;
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
            first.WriteXml(writer);
            second.WriteXml(writer);
            writer.WriteEndElement();
        }
        #endregion

        #endregion
    }
}