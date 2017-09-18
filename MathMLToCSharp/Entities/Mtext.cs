using System.Text;

namespace MathMLToCSharp.Entities
{
  /// <summary>
  /// Text.
  /// </summary>
  public class Mtext : WithTextContent
  {
    public Mtext(string content) : base(content) {}

    public override void Visit(StringBuilder sb, BuildContext bc)
    {
      base.Visit(sb, bc);
    }
  }
}