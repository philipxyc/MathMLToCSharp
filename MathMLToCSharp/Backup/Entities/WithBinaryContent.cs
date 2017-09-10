using System.Text;
using Wintellect.PowerCollections;

namespace MathMLToCSharp.Entities
{
  public abstract class WithBinaryContent : IBuildable
  {
    protected readonly IBuildable first, second;
    protected WithBinaryContent(IBuildable first, IBuildable second)
    {
      this.first = first;
      this.second = second;
    }

    public Pair<IBuildable, IBuildable> Contents
    {
      get
      {
        return new Pair<IBuildable, IBuildable>(first, second);
      }
    }

    #region IBuildable Members

    public abstract void Visit(StringBuilder sb, BuildContext bc);

    #endregion
  }
}