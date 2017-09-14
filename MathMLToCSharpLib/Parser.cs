using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using MathMLToCSharpLib.Entities;
using Math=MathMLToCSharpLib.Entities.Math;

namespace MathMLToCSharpLib
{
  public static class Parser
  {
    /// <summary>
    /// Capitalizes the initial letter of an identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>The modified identifier.</returns>
    internal static string camel(string id)
    {
      return "" + char.ToUpper(id[0], CultureInfo.InvariantCulture) + id.Substring(1);
    }

    /// <summary>
    /// Parses the XML tree and returns the root <c>Math</c> element.
    /// </summary>
    /// <param name="rootElement">The root <c>XElement</c></param>
    /// <returns>A parsed tree if everything went ok; <c>null</c> otherwise.</returns>
    public static Math Parse(XElement rootElement)
    {
      return GenericParse(rootElement) as Math;
    }


    /// <summary>
    /// Parses the element and creates a parse tree
    /// </summary>
    /// <param name="element">The element to parse from.</param>
    /// <returns>The root of the parse tree.</returns>
    public static IBuildable GenericParse(XElement element)
    {
      // make children recursively depending on the number of contained elements
      var elems = element.Elements();
      var c = camel(element.Name.LocalName);
      var t = Type.GetType("MathMLToCSharpLib.Entities." + c);
      if (t == null)
      {
        throw new Exception("Type " + c + " has not yet been implemented.");
      }
      switch (elems.Count())
      {
        case 0: // return text
          Trace.WriteLine(String.Format(CultureInfo.InvariantCulture, "Creating {0} with 0 params.", t.Name));
          return Activator.CreateInstance(t, element.Value) as IBuildable;
        case 1:
          Trace.WriteLine(String.Format(CultureInfo.InvariantCulture, "Creating {0} with 1 param.", t.Name));
          var first = GenericParse(elems.First());
          return Activator.CreateInstance(t, first) as IBuildable;
        case 2:
          var pair = elems.Select(e => GenericParse(e)).ToArray();
          IBuildable result;
          // some cases require a binary constructor; other cases requre an N-ary one instead
          try
          {
            result = Activator.CreateInstance(t, pair[0], pair[1]) as IBuildable;
          } catch
          {
            result = Activator.CreateInstance(t, new[] { pair }) as IBuildable;
          }
          return result;
        default: // has a single item
          var pars = elems.Select(e => GenericParse(e)).ToArray();
          return Activator.CreateInstance(t, new [] { pars }) as IBuildable;
      }
    }
  }
}
