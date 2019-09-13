using System;
using System.Text;
using System.Xml.Linq;

using MathMLToCSharpLib;
using MathMLToCSharpLib.Entities;

using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ConversionTests
    {
        private static void Main() { }

        private static string ParseAndOutput(string mathML)
        {
            return ParseAndOutput(mathML, new BuildContextOptions());
        }

        private static string ParseAndOutput(string mathML, BuildContextOptions options)
        {
            IBuildable b = Parser.Parse(XElement.Parse(mathML));
            StringBuilder sb = new StringBuilder();
            b.Visit(sb, new BuildContext(options));
            return sb.ToString();
        }

        [Test]
        public void EmptyMathTest()
        {
            Assert.AreEqual(
              "",
              ParseAndOutput("<math abra=\"cadabra\"></math>"),
              "Empty math element must render to nothing.");
        }

        [Test]
        public void MathWithNumberTest()
        {
            Assert.AreEqual("2;",
                            ParseAndOutput("<math><mn>2</mn></math>"),
                            "Basic mn block must render properly.");
        }

        [Test]
        public void BasicAssignmentTest()
        {
            Assert.AreEqual(
              "double n = 0.0;" + Environment.NewLine + "n=2;",
              ParseAndOutput("<math><mi>n</mi><mo>=</mo><mn>2</mn></math>"),
              "Declaration and assignment ought to be together.");
        }

        [Test]
        public void BasicSubscriptTest()
        {
            Assert.AreEqual(
              "double a_b = 0.0;" + Environment.NewLine + "a_b;",
              ParseAndOutput("<math><msub><mi>a</mi><mi>b</mi></msub></math>"));
        }

        [Test]
        public void DoubleSubscriptTest()
        {
            Assert.AreEqual(
              "double a_b_c = 0.0;" + Environment.NewLine + "a_b_c;",
              ParseAndOutput(@"<math display='block'>
 
   <msub>
    <mi>a</mi>
    <mrow>
     <msub>
      <mi>b</mi>
      <mi>c</mi>
     </msub>
     
    </mrow>
   </msub>
   
 
</math>
"));
        }
    }
}
