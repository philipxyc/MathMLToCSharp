using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace MathMLToCSharpLib.Entities
{
    public class BuildablePlainSum : ISum
    {
        private readonly IBuildable expression;
        public BuildablePlainSum(IBuildable expression)
        {
            this.expression = expression;
        }

        #region ISum Members

        public void Visit(StringBuilder sb, BuildContext bc)
        {
            sb.Append(bc.GetSumIdentifier(this));
        }

        public string Expression(BuildContext context)
        {
            // get the target
            StringBuilder target = new StringBuilder();
            expression.Visit(target, context);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("for (int i{0} = 0; i{0} < {1}.Length; ++i{0})",
                            context.GetSumIdentifier(this), target);
            sb.AppendLine();
            sb.Append("  ");
            sb.AppendFormat("{0} += {1}[i{0}];", context.GetSumIdentifier(this), target);
            return sb.ToString();
        }

        /*public XElement ToXElement()
        {
            return new XElement(this.GetType().Name, expression.ToXElement());
        }*/

        #region IXmlSerializable Members
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            expression.ReadXml(reader);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(this.GetType().Name);
            expression.WriteXml(writer);
            writer.WriteEndElement();
        }
        #endregion

        #endregion
    }
}
