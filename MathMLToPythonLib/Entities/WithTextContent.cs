using System.Text;

namespace MathMLToPythonLib.Entities
{
  public abstract class WithTextContent : IBuildable
  {
    protected string content;
    protected WithTextContent(string content) {
      this.content = content;
    }

    public string Content
    {
      get { return content; }
      set { content = value; }
    }

    #region IBuildable Members

    public virtual void Visit(StringBuilder sb, BuildContext bc)
    {
      bc.Tokens.Add(this);
      sb.Append(content);
    }

    #endregion
  }
}