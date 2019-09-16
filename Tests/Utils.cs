using System.Text;
using System.Xml.Linq;

using MathMLToCSharpLib.Entities;

namespace MathMLToCSharpLib.Tests
{
    public static class Utils
    {
        public static string ParseAndOutput(string mathML, BuildContextOptions options)
        {
            IBuildable b = Parser.Parse(XElement.Parse(mathML));
            StringBuilder sb = new StringBuilder();
            b.Visit(sb, new BuildContext(options));
            return sb.ToString();
        }


    }
}
