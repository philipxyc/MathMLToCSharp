using System.Text;

namespace MathMLToPythonLib.Entities
{
    /// <summary>
    /// Table cell.
    /// </summary>
    public class Mtd : WithBuildableContents
    {
        public Mtd(IBuildable content) : base(new[] { content }) { }
        public Mtd(IBuildable[] contents) : base(contents) { }
        public override void Visit(StringBuilder sb, BuildContext bc)
        {
            bc.Tokens.Add(this);
            foreach (var content in contents)
            {
                content.Visit(sb, bc);
            }
        }
    }
}