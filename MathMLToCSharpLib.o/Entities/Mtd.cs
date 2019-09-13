using System.Text;

namespace MathMLToCSharpLib.Entities
{
    /// <summary>
    /// Table cell.
    /// </summary>
    public class Mtd : WithBuildableContent
    {
        public Mtd() { }
        public Mtd(IBuildable content) : base(content) { }
        public override void Visit(StringBuilder sb, BuildContext bc)
        {
            bc.Tokens.Add(this);
            content.Visit(sb, bc);
        }
    }
}