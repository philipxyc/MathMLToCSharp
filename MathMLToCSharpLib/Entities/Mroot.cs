using System.Text;

namespace MathMLToCSharpLib.Entities
{
    /// <summary>
    /// Radical. These do not exist in C#, so cubic root of N is encoded as <c>Math.Pow(N, 1/3)</c>.
    /// </summary>
    public class Mroot : WithBinaryContent
    {
        public Mroot() { }
        public Mroot(IBuildable first, IBuildable second) : base(first, second) { }

        public override void Visit(StringBuilder sb, BuildContext bc)
        {
            sb.Append("Math.Pow(");
            first.Visit(sb, bc);
            sb.Append(", 1 / ");
            second.Visit(sb, bc);
            sb.Append(")");
        }
    }
}