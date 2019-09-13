using System.Text;

namespace MathMLToCSharpLib.Entities
{
    /// <summary>
    /// Number.
    /// </summary>
    public class Mn : WithTextContent
    {
        public Mn() { }
        public Mn(string content) : base(content) { }

        /// <summary>
        /// Returns <c>true</c> if the content is an integer greater than 1 (i.e., 2, 3, etc.)
        /// </summary>
        public bool IsIntegerGreaterThan1
        {
            get
            {
                try
                {
                    double d = double.Parse(content);
                    if (System.Math.Floor(d) == d)
                        if (d > 1.0)
                            return true;
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public int IntegerValue
        {
            get
            {
                return int.Parse(content);
            }
        }

        public override void Visit(StringBuilder sb, BuildContext bc)
        {
            base.Visit(sb, bc);
            if (bc.Options.NumberPostfix && !bc.Options.SubscriptMode)
                sb.Append(Semantics.postfixForDataType(bc.Options.EqnDataType));
        }
    }
}