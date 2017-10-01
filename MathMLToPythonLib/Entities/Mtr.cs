using System.Text;

namespace MathMLToPythonLib.Entities
{
    /// <summary>
    /// Table row.
    /// </summary>
    public class Mtr : WithBuildableContents
    {
        public Mtr(IBuildable[] contents) : base(contents) { }
        public Mtr(IBuildable contents) : base(new[] { contents }) { }
        public Mtr(IBuildable first, IBuildable second) : base(new[] { first, second }) { }
        public override void Visit(StringBuilder sb, BuildContext context)
        {
            sb.Append("[");
            for (int i = 0; i < contents.Length; ++i)
            {
                contents[i].Visit(sb, context);
                if (i + 1 != contents.Length)
                    sb.Append(", ");
            }
            sb.Append("]");
        }
        public bool ContainsSingleColumn
        {
            get
            {
                return contents.Length == 1 && contents[0] is Mtd;
            }
        }
    }
}