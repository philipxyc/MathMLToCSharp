using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MathMLToCSharpLib.Entities
{
  /// <summary>
  /// Subscript and superscript one one variable.
  /// </summary>
  public class Msubsup : WithBuildableContents
  {
    public Msubsup(IBuildable[] contents)
    { // handle star superscript
      List<IBuildable> localCopy = new List<IBuildable>(contents);
      if (localCopy.Count == 3)
      {
        Mo mo = localCopy[2] as Mo;
        if (mo != null && mo.IsTimesOrStar)
        {
          Mi subscript = localCopy[1] as Mi;
          if (subscript != null)
          {
            subscript.Content += Semantics.starPrefix;
            localCopy.RemoveAt(2);
          } else
          {
            // maybe the subscript is an mrow
            Mrow row = localCopy[1] as Mrow;
            if (row != null && row.LastElement != null && row.LastElement is WithTextContent)
            {
              WithTextContent lastElem = (WithTextContent)row.LastElement;
              lastElem.Content += Semantics.starPrefix;
              localCopy.RemoveAt(2);
            }
          }
        }
      }
      base.contents = localCopy.ToArray();
    }
    public override void Visit(StringBuilder sb, BuildContext context)
    {
      Debug.Assert(contents.Length == 2 || contents.Length == 3);
      Msub sub = new Msub(contents[0], contents[1]);
      switch (contents.Length) {
        case 2:
          sub.Visit(sb, context);
          break;
        case 3:
          Msup sup = new Msup(sub, contents[2]);
          sup.Visit(sb, context);
          break;
        default:
          throw new ApplicationException("Incorrect number of arguments in Msubsup");
      }
    }
  }
}