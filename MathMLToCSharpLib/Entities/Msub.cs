using System.Text;

namespace MathMLToCSharpLib.Entities
{
    /// <summary>
    /// Subscript.
    /// </summary>
    public class Msub : WithBinaryContent
    {
        public Msub() { }
        public Msub(IBuildable first, IBuildable second) : base(first, second) { }

        public override void Visit(StringBuilder sb, BuildContext bc)
        {
            bc.Tokens.Add(this);

            bool last = bc.Options.SubscriptMode;
            bc.Options.SubscriptMode = true;

            StringBuilder b = new StringBuilder();

            first.Visit(b, bc);
            b.Append("_");
            second.Visit(b, bc);

            string varName = b.ToString();
            if (!bc.Vars.Contains(varName))
                if (!last) // unless we are already in a subscript-entering mode...
                    bc.Vars.Add(varName);

            sb.Append(varName);

            bc.Options.SubscriptMode = last;
        }
    }
}