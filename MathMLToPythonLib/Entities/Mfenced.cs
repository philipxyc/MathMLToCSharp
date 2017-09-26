using System.Text;

namespace MathMLToPythonLib.Entities
{
  /// <summary>
  /// Code in brackets.
  /// </summary>
  public class Mfenced : WithBuildableContent
  {
    public Mfenced(IBuildable content) : base(content) {}
    public override void Visit(StringBuilder sb, BuildContext bc)
    {
      bc.Tokens.Add(this);

      if (bc.LastTokenRequiresTimes)
        sb.Append("*");

      // todo: brackets do not need to be added if there is only one element (e.g., braces around a matrix)
      sb.Append("(");
      base.Visit(sb, bc);
      sb.Append(")");
    }
  }
}