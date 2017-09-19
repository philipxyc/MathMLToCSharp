using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

namespace MathMLToCSharp.Entities
{
  /// <summary>
  /// Operator.
  /// </summary>
  public class Mo : WithTextContent
  {
    private static readonly StringDictionary rep;
    static Mo()
    {
      // replacements in lieu of the actual operator
      rep = new StringDictionary
      {
        {"!", ".Factorial()"}, 
        {"-", "-"}, // this one is *really* nasty :)
        {"−", "-"}, // this one is also annoying (thanks MathType)
        {"÷", "/"}, 
        {"×", "*"},
        {"∗", "*"}, // << unpleasant too!
        {"[", "("}, 
        {"]", ")"},
        {"{", "("},
        {"}", ")"}
      };
    }

    public Mo(string content) : base(content) {}

    /// <summary>
    /// Returns <c>true</c> if this is an equals operator.
    /// </summary>
    /// <remarks>
    /// Useful for determining whether we are on the RHS or LHS of an assignment.
    /// </remarks>
    public bool IsEquals
    {
      get
      {
        return content == "=";
      }
    }

    /// <summary>
    /// Returns <c>true</c> if this is a closing brace.
    /// </summary>
    public bool IsClosingBrace
    {
      get
      {
        return content == ")";
      }
    }

    /// <summary>
    /// Returns <c>true</c> if this is an opening brace.
    /// </summary>
    public bool IsOpeningBrace
    {
      get
      {
        return content == "(";
      }
    }

    /// <summary>
    /// Returns <c>true</c> if this is a printable operator.
    /// </summary>
    public bool IsPrintableOperator
    {
      get
      {
        return (content != "\u2061" && content != "\u2062" && content != "\u2063");
      }
    }

    /// <summary>
    /// Gets a value indicating whether the content is uppercase sigma.
    /// </summary>
    /// <value><c>true</c> if the content is sigma; otherwise, <c>false</c>.</value>
    public bool IsSigma
    {
      get
      {
        return content == "∑";
      }
    }

    /// <summary>
    /// Gets a value indicating whether the content is uppercase delta.
    /// </summary>
    /// <value><c>true</c> if the content is delta; otherwise, <c>false</c>.</value>
    public bool IsDelta
    {
      get
      {
        return content == "∆";
      }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is times or star.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is times or star; otherwise, <c>false</c>.
    /// </value>
    public bool IsTimesOrStar
    {
      get
      {
        return "*∗".Contains(content);
      }
    }

    public override void Visit(StringBuilder sb, BuildContext bc)
    {
      bc.Tokens.Add(this);

      // if we are in subscript mode, adding an operator is not necessary, but neither can we ignore it
      if (bc.Options.SubscriptMode)
      {
        sb.Append("_");
      }
      else if (rep.ContainsKey(content))
        sb.Append(rep[content]);
      else
      { 
        Trace.Assert(content.Length == 1, "We only support 1-char operators (for now).");
        if (IsPrintableOperator)
        {
          if (IsOpeningBrace && bc.LastTokenRequiresTimes)
            sb.Append("*");

          sb.Append(content);
        }
      }
    }
  }
}