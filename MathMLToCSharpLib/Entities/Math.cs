using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MathMLToCSharpLib.Entities
{
    /// <summary>
    /// Root element in MathML. Not to be confused with <c>System.Math</c>.
    /// </summary>
    public class Math : WithBuildableContents, IXmlSerializable
    {
        public Math() { }
        public Math(string content) : base(new IBuildable[] { }) { /* just in case */ }
        public Math(IBuildable content) : base(new[] { content }) { }
        public Math(IBuildable[] contents) : base(contents) { }

        #region IXmlSerializable Methods
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            List<IBuildable> tempContent = new List<IBuildable>();
            while(reader.Read())
            {
                IBuildable element = null;
                if (reader.IsStartElement() & reader.Name != "Math")
                {
                    Type type = Type.GetType(this.GetType().Namespace + "." + reader.Name);
                    element = (IBuildable)Activator.CreateInstance(type);
                    element.ReadXml(reader);
                    tempContent.Add(element);
                }
                else if (reader.NodeType == XmlNodeType.EndElement & reader.Name == this.GetType().Name)
                {
                    contents = tempContent.ToArray();
                }
            }
            
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(this.GetType().Name);
            foreach (IBuildable item in contents)
                item.WriteXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
        #endregion

        public override void Visit(StringBuilder sb, BuildContext context)
        {
            base.Visit(sb, context);

            // todo: this assumes that there is only one statement, which may not be the case
            if (sb.ToString().Length > 0)
                sb.Append(";");

            // sums
            int j = context.Sums.Count;
            if (j > 0)
            {
                var builder = new StringBuilder();
                foreach (var v in context.Sums)
                {
                    builder.AppendLine(v.First.Expression(context));
                }
                sb.Insert(0, builder.ToString());
            }

            // variables
            int i = context.Vars.Count;
            if (i > 0)
            {
                var builder = new StringBuilder();
                foreach (var v in context.Vars)
                {
                    builder.Append(Enum.GetName(typeof(EquationDataType), context.Options.EqnDataType).ToLower());
                    builder.Append(" ");
                    builder.Append(v);
                    builder.Append(" = 0.0"); // this is a *must*
                    if (context.Options.NumberPostfix)
                        builder.Append(Semantics.postfixForDataType(context.Options.EqnDataType));
                    builder.AppendLine(";");
                }
                foreach (IBuildable ib in context.PossibleDivisionsByZero)
                {
                    var b = new StringBuilder();
                    ib.Visit(b, new BuildContext(Singleton.Instance.Options));
                    builder.AppendFormat("Debug.Assert({0} != 0, \"Expression {0} is about to cause division by zero.\");",
                      b);
                    builder.AppendLine();
                }
                sb.Insert(0, builder.ToString());
            }
        }


    }
}