using System.Text;

namespace MathMLToCSharpLib.Entities
{
    class Munderover : WithBuildableContents
    {
        public Munderover() { }
        public Munderover(IBuildable[] contents) : base(contents) { }
        public override void Visit(StringBuilder sb, BuildContext context)
        {
            if (contents.Length == 3)
            {
                // is the first one an operator?
                Mo mo = contents[0] as Mo;
                if (mo != null)
                {
                    if (mo.IsSigma)
                    {
                        //todo: summation!

                    }
                }
            }
            else
            {
                sb.Append("Munderover must have 3 items");
            }
        }
    }
}
