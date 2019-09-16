using NUnit.Framework;

namespace MathMLToCSharpLib.Tests
{
    public class SimpleEquations
    {
        /// <summary>
        /// https://en.wikipedia.org/wiki/Froude_number
        /// </summary>
        [Test]
        public void Modified_FroudeNumber_Equation()
        {
            const string input =
                    @"<math xmlns=""http://www.w3.org/1998/Math/MathML"" alttext=""F_{n}=\frac{0,515*V_{m}}{\Sqrt{g⋅L_{WL}}}"" display=""block"">
  <msub>
    <mi>F</mi>
    <mrow>
      <mi>n</mi>
    </mrow>
  </msub>
  <mo>=</mo>
  <mfrac>
    <mrow>
      <mn>0</mn>
      <mo>,</mo>
      <mn>5</mn>
      <mn>1</mn>
      <mn>5</mn>
  <mo>*</mo>   <msub>
        <mi>V</mi>
        <mrow>
          <mi>m</mi>
        </mrow>
      </msub>
    </mrow>
    <mrow>
      <msqrt>
        <mrow>
          <mi>g</mi>
          <mo>⋅<!-- \cdot --></mo>
          <msub>
            <mi>L</mi>
            <mrow>
              <mi>W</mi>
              <mi>L</mi>
            </mrow>
          </msub>
        </mrow>
      </msqrt>
    </mrow>
  </mfrac>
</math>"
                ;

            var result = Utils.ParseAndOutput(input, new BuildContextOptions()
            {
                InitializeVariables = false,
                MaxInlinePower = 1
            });
            Assert.AreEqual(@"F_n=((0.515*V_m) / (Math.Sqrt(g*L_WL)));", result);
        }



    }
}
