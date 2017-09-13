using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace MathMLToCSharp.Entities
{
    /// <summary>
    /// A table. Typically used for matrices.
    /// </summary>
    public class Mtable : WithBuildableContents
    {
        static int level = 0;
        public Mtable(IBuildable[] contents) : base(contents) { }
        public override void Visit(StringBuilder sb, BuildContext context)
        {
            level++;
            Matrix<double> m = DenseMatrix.OfArray(new double[,] { { 1, 2 }, { 3, 4 } });
            var c = m.Evd().EigenVectors;
            if (level == 1)
                sb.Append("DenseMatrix.OfArray(new double[,] {");//matrix
            else
                sb.Append("{");//matrix
            for (int i = 0; i < contents.Length; ++i)
            {
                contents[i].Visit(sb, context);
                if (i + 1 != contents.Length)
                    sb.Append(", ");
            }
            if (level == 1)
                sb.Append("})");
            else
                sb.Append("}");
            level--;
        }
    }
}