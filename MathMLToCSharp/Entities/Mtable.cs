using System.Text;

namespace MathMLToCSharp.Entities
{
  /// <summary>
  /// A table. Typically used for matrices.
  /// </summary>
  public class Mtable : WithBuildableContents
  {
    public Mtable(IBuildable[] contents) : base(contents) {}
    public override void Visit(StringBuilder sb, BuildContext context)
    {
      sb.Append("{");//matrix
      for (int i = 0; i < contents.Length; ++i)
      {
        contents[i].Visit(sb, context);
        if (i + 1 != contents.Length)
          sb.Append(", ");
      }
      sb.Append("}");
    }
  }
}