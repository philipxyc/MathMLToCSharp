using System;
using System.Text;

namespace MathMLToPythonLib.Entities
{
    /// <summary>
    /// Root element in MathML. Not to be confused with <c>System.Math</c>.
    /// </summary>
    public class Math : WithBuildableContents
    {
        public Math(string content) : base(new IBuildable[] { }) { /* just in case */ }
        public Math(IBuildable content) : base(new[] { content }) { }
        public Math(IBuildable[] contents) : base(contents) { }
        public override void Visit(StringBuilder sb, BuildContext context)
        {
            if (contents.Length == 1 && contents[0] is Mtable && (contents[0] as Mtable).ContainsSingleColumn)
                context.MultiLineMatrix = true;
            base.Visit(sb, context);

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
                //below is to create var in C#

                //  var builder = new StringBuilder();
                //  foreach (var v in context.Vars)
                //  {
                //    builder.Append(Enum.GetName(typeof(EquationDataType), context.Options.EqnDataType).ToLower());
                //    builder.Append(" ");
                //    builder.Append(v);
                //    builder.Append(" = 0.0"); // this is a *must*
                //    if (context.Options.NumberPostfix)
                //      builder.Append(Semantics.postfixForDataType(context.Options.EqnDataType));
                //    builder.AppendLine(";");
                //  }
                //  foreach (IBuildable ib in context.PossibleDivisionsByZero)
                //  {
                //    var b = new StringBuilder();
                //    ib.Visit(b, new BuildContext(Singleton.Instance.Options));
                //    builder.AppendFormat("Debug.Assert({0} != 0, \"Expression {0} is about to cause division by zero.\");", 
                //      b);
                //    builder.AppendLine();
                //  }
                //  sb.Insert(0, builder.ToString());
            }
        }
        public bool ContainSingleMtable
        {
            get
            {
                return contents.Length == 1 && contents[0] is Mtable;
            }
        }
    }
}