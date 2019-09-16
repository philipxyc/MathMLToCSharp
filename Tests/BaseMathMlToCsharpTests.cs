
using NUnit.Framework;

namespace MathMLToCSharpLib.Tests
{
    public class BaseMathMlToCsharpTests
    {
        /// <image url="images/Call_LaTeX_To_MathML_Einstein_Equation.png" />
        [Test]
        public void A010_MathML_Einstein_Equation()
        {
            const string input = @"<math xmlns=""http://www.w3.org/1998/Math/MathML"" alttext=""E=m{c^{2}}"" display=""block"">
  <mi>E</mi>
  <mo>=</mo>
  <mi>m</mi>
  <msup>
    <mi>c</mi>
    <mrow>
      <mn>2</mn>
    </mrow>
  </msup>
</math>"
                ;

            var result = Utils.ParseAndOutput(input, new BuildContextOptions()
            {
                InitializeVariables = false,
                MaxInlinePower = 1
            });
            Assert.AreEqual(@"E=m*Math.Pow(c, 2);", result);
        }

        /// <image url="images/Call_LaTeX_To_MathML_Einstein_Equation.png" />
        [Test]
        public void A015_MathML_Einstein_Equation()
        {
            const string input = @"<math xmlns=""http://www.w3.org/1998/Math/MathML"" alttext=""E=m{c^{2}}"" display=""block"">
  <mi>E</mi>
  <mo>=</mo>
  <mi>m</mi>
  <msup>
    <mi>c</mi>
    <mrow>
      <mn>2</mn>
    </mrow>
  </msup>
</math>"
                ;

            var result = Utils.ParseAndOutput(input, new BuildContextOptions()
            {
                InitializeVariables = false,
                MaxInlinePower = 2
            });
            Assert.AreEqual(@"E=m*c*c;", result);
        }

        /// <image url="images/Call_LaTeX_To_MathML_Einstein_Equation.png" />
        [Test]
        public void A020_MathML_EinsteinH_Equation()
        {
            // Note: it contains an 'invisible times' unicode character \u2062
            const string input = @"<math xmlns=""http://www.w3.org/1998/Math/MathML"" alttext=""E=m⁢c^{2}"" display=""block"">
  <mi>E</mi>
  <mo>=</mo>
  <mi>m</mi>
  <mo>⁢<!-- \Hidden Multiplier --></mo>
  <msup>
    <mi>c</mi>
    <mrow>
      <mn>2</mn>
    </mrow>
  </msup>
</math>";
            var result = Utils.ParseAndOutput(input, new BuildContextOptions()
            {
                InitializeVariables = false,
                MaxInlinePower = 1
            });
            Assert.AreEqual(@"E=m*Math.Pow(c, 2);", result);
        }

        [Test]
        public void A030_MathML_Einstein_Mass_Equation()
        {
            const string input = @"<math xmlns=""http://www.w3.org/1998/Math/MathML"" alttext=""E_{nergy}=M_{ass}\cdot c^2_{lightSpeed}"" display=""block"">
  <msub>
    <mi>E</mi>
    <mrow>
      <mi>n</mi>
      <mi>e</mi>
      <mi>r</mi>
      <mi>g</mi>
      <mi>y</mi>
    </mrow>
  </msub>
  <mo>=</mo>
  <msub>
    <mi>M</mi>
    <mrow>
      <mi>a</mi>
      <mi>s</mi>
      <mi>s</mi>
    </mrow>
  </msub>
  <mo>⋅<!-- \cdot --></mo>
  <msubsup>
    <mi>c</mi>
    <mrow>
      <mi>l</mi>
      <mi>i</mi>
      <mi>g</mi>
      <mi>h</mi>
      <mi>t</mi>
      <mi>S</mi>
      <mi>p</mi>
      <mi>e</mi>
      <mi>e</mi>
      <mi>d</mi>
    </mrow>
    <mrow>
      <mn>2</mn>
    </mrow>
  </msubsup>
</math>"
            ;

            var result = Utils.ParseAndOutput(input, new BuildContextOptions()
            {
                InitializeVariables = false,
                MaxInlinePower = 1
            });
            Assert.AreEqual(@"E_nergy=M_ass*Math.Pow(c_lightSpeed, 2);", result);
        }


        [Test]
        public void A040_CreateGrouped_Variables()
        {
            // Note: Still waiting for a fix for https://github.com/verybadcat/CSharpMath/issues/59
            const string input =
                    @"<math xmlns=""http://www.w3.org/1998/Math/MathML"" alttext=""\mathrm{Gap}^2"" display=""block"">
  <msup>
    <mi>Gap</mi>
    <mrow>
      <mn>2</mn>
    </mrow>
  </msup>
</math>"
                ;
            var result = Utils.ParseAndOutput(input, new BuildContextOptions()
            {
                InitializeVariables = false,
                MaxInlinePower = 1
            });
            Assert.AreEqual(@"Math.Pow(Gap, 2);", result);
        }

        [Test]
        public void A041_CreateGrouped_Variables()
        {
            // Note: Still waiting for a fix for https://github.com/verybadcat/CSharpMath/issues/59
            const string input =
                    @"<math xmlns=""http://www.w3.org/1998/Math/MathML"" alttext=""\mathrm{Gap}^2\mathit{gb}"" display=""block"">
  <msup>
    <mi>Gap</mi>
    <mrow>
      <mn>2</mn>
    </mrow>
  </msup>
  <mi>gb</mi>
</math>"
                ;
            var result = Utils.ParseAndOutput(input, new BuildContextOptions()
            {
                InitializeVariables = false,
                MaxInlinePower = 1
            });
            Assert.AreEqual(@"Math.Pow(Gap, 2)*gb;", result);
        }


        /// <image url="images/Grouped_Variable.png" />
        [Test]
        public void A045_CreateGrouped_Variable()
        {
            // Note: Still waiting for a fix for https://github.com/verybadcat/CSharpMath/issues/59
            const string input =
                    @"<math xmlns=""http://www.w3.org/1998/Math/MathML"" alttext=""F_n=\mathrm{Gap}^2"" display=""block"">
  <msub>
    <mi>F</mi>
    <mrow>
      <mi>n</mi>
    </mrow>
  </msub>
  <mo>=</mo>
  <msup>
    <mi>Gap</mi>
    <mrow>
      <mn>2</mn>
    </mrow>
  </msup>
</math>"
                ;
            var result = Utils.ParseAndOutput(input, new BuildContextOptions()
            {
                InitializeVariables = false,
                MaxInlinePower = 1
            });
            Assert.AreEqual(@"F_n=Math.Pow(Gap, 2);", result);
        }



    }
}
