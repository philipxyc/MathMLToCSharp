namespace MathMLToCSharpLib.Entities
{
  class Mstyle : WithBuildableContents
  {
    public Mstyle(IBuildable content) : base(new[]{content}) {}
    public Mstyle(IBuildable[] contents) : base(contents) {}
  }
}
