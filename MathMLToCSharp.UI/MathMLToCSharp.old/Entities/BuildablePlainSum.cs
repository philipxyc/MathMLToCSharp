using System.Text;

namespace MathMLToCSharp.Entities
{
  public class BuildablePlainSum : ISum
  {
    private readonly IBuildable expression;
    public BuildablePlainSum(IBuildable expression)
    {
      this.expression = expression;
    }

    #region ISum Members

    public void Visit(StringBuilder sb, BuildContext bc)
    {
      sb.Append(bc.GetSumIdentifier(this));
    }

    public string Expression(BuildContext context)
    {
      // get the target
      StringBuilder target = new StringBuilder();
      expression.Visit(target, context);

      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("for (int i{0} = 0; i{0} < {1}.Length; ++i{0})",
                      context.GetSumIdentifier(this), target);
      sb.AppendLine();
      sb.Append("  ");
      sb.AppendFormat("{0} += {1}[i{0}];", context.GetSumIdentifier(this), target);
      return sb.ToString();
    }

    #endregion
  }
}
