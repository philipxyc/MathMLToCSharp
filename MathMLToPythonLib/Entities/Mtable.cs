using System.Text;

namespace MathMLToPythonLib.Entities
{
    /// <summary>
    /// A table. Typically used for matrices.
    /// </summary>
    public class Mtable : WithBuildableContents
    {
        static int level = 0;
        public Mtable(IBuildable[] contents) : base(contents) { }
        public Mtable(IBuildable contents) : base(new[] { contents }) { }
        public override void Visit(StringBuilder sb, BuildContext context)
        {
            if (context.MultiLineMatrix == 0)
            {
                context.MultiLineMatrix = 1;
                for (int i = 0; i < contents.Length; ++i)
                {
                    var nsb = new StringBuilder();
                    contents[i].Visit(nsb, context);
                    sb.Append(nsb.Remove(0, 1).Remove(nsb.Length - 1, 1));// remove "[" & "]"
                    if (i + 1 != contents.Length)
                        sb.Append(";");
                }
            }
            else
            {
                level++;
                if (level == 1)
                {
                    sb.Append("numpy.matrix([");//matrix
                }
                else
                    sb.Append("[");//matrix
                for (int i = 0; i < contents.Length; ++i)
                {
                    contents[i].Visit(sb, context);
                    if (i + 1 != contents.Length)
                        sb.Append(", ");
                }
                if (level == 1)
                {
                    sb.Append("])");
                }
                else
                    sb.Append("]");
                level--;

            }
        }
        public bool ContainsSingleColumn
        {
            get
            {
                return (contents[0] as Mtr).ContainsSingleColumn;
            }
        }
    }
}