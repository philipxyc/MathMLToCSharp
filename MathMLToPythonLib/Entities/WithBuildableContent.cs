using System.Text;

namespace MathMLToPythonLib.Entities
{
  public abstract class WithBuildableContent : IBuildable
  {
    protected readonly IBuildable content;
    protected WithBuildableContent(IBuildable content)
    {
      this.content = content;
    }

    #region IBuildable Members

    public virtual void Visit(StringBuilder sb, BuildContext bc)
    {
      bc.Tokens.Add(this);
      content.Visit(sb, bc);
    }

    #endregion
  }
}