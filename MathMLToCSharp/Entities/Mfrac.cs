using System.Text;

namespace MathMLToCSharp.Entities
{
  /// <summary>
  /// Fraction.
  /// </summary>
  // note: it might be worthwhile somehow differentiating between fraction and
  // ordinary division, e.g., having fraction parts evaluate as temporary variables.
  public class Mfrac : WithBinaryContent
  {
    public Mfrac(IBuildable first, IBuildable second) : base(first, second) {}

    public override void Visit(StringBuilder sb, BuildContext bc)
    {
      bc.Tokens.Add(this);

      bool needReduce = false;
      // note: the use of double is a judgement call here
      double result = 0.0;
      if (bc.Options.ReduceFractions)
      {
        Mrow row1 = first as Mrow;
        Mrow row2 = second as Mrow;
        if (row1 != null && row2 != null && row1.Contents.Length > 0 && row2.Contents.Length > 0)
        {
          Mn mn1 = row1.Contents[0] as Mn;
          Mn mn2 = row2.Contents[0] as Mn;
          if (mn1 != null && mn2 != null)
          {
            try
            {
              double _1, _2;
              if (double.TryParse(mn1.Content, out _1) &&
                  double.TryParse(mn2.Content, out _2) &&
                  _2 != 0.0)
              {
                result = _1/_2;
                needReduce = true;
              }
            }
            catch
            {
            }
          }
        }
      }

      if (needReduce)
      {
        sb.Append(result);
      }
      else
      {
        sb.Append("((");
        first.Visit(sb, bc);
        sb.Append(") / (");
        second.Visit(sb, bc);
        sb.Append("))");

        // add to checks
        bc.PossibleDivisionsByZero.Add(second);
      }
    }
  }
}