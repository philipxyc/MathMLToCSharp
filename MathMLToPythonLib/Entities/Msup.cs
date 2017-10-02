using System.Text;
using Wintellect.PowerCollections;

namespace MathMLToPythonLib.Entities
{
    /// <summary>
    /// Superscript. Taken to mean 'power of' unless the superscript parses into a *.
    /// </summary>
    public class Msup : WithBinaryContent
    {
        public Msup(IBuildable first, IBuildable second) : base(first, second) { }

        public Pair<IBuildable, IBuildable> Values
        {
            get
            {
                return new Pair<IBuildable, IBuildable>(first, second);
            }
        }

        public override void Visit(StringBuilder sb, BuildContext bc)
        {
            if (bc.Options.SubscriptMode)
            {
                // separate by double underscore to differentiate from Msub and prevent clashes
                first.Visit(sb, bc);
                sb.Append("__");
                second.Visit(sb, bc);
            }
            else if (second is Mo && (second as Mo).IsTimesOrStar && first is Mi)
            {
                // this isn't really a superscript - it's part of the variable
                Mi mi = (Mi)first;
                Mi newMi = new Mi(mi.Content + Semantics.starPrefix);
                newMi.Visit(sb, bc);
            }
            else
            {
                if ((first is Mrow) && ((first as Mrow).ContainsSingleMatrix) || ((first as Mrow).ContainsSingleMi) ||
                    (((first as Mrow).Contents[0] is Mrow) && ((first as Mrow).Contents[0] as Mrow).ContainsSingleMatrix))
                {
                    if ((second is Mrow) && ((second as Mrow).ContainsSingleMi) && ((second as Mrow).LastElement as Mi).Content == "T")
                    {
                        first.Visit(sb, bc);
                        sb.Append(".T");
                    }
                    bc.Tokens.Add(this);
                    return;
                }

                if (bc.LastTokenRequiresTimes)
                    sb.Append("*");

                first.Visit(sb, bc);
                sb.Append("**");
                second.Visit(sb, bc);
                sb.Append("");

            }

            bc.Tokens.Add(this);
        }
    }
}