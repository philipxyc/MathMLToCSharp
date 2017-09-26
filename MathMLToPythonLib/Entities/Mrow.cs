using System.Text;

namespace MathMLToPythonLib.Entities
{
    /// <summary>
    /// A fairly generic element that does not mean anything.
    /// </summary>
    public class Mrow : WithBuildableContents
    {
        public Mrow(IBuildable[] content) : base(content)
        {

        }

        public Mrow(IBuildable first, IBuildable second) : this(new[] { first, second })
        {
        }

        public Mrow(IBuildable content) : this(new[] { content })
        {
        }

        public IBuildable[] Contents
        {
            get { return contents; }
        }

        /// <summary>
        /// Returns <c>true</c> if this <see cref="Mrow"/> contains a single <see cref="Mi"/>.
        /// </summary>
        public bool ContainsSingleMi
        {
            get
            {
                return contents.Length == 1 && contents[0] is Mi;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this Mrow contains a single Mn.
        /// </summary>
        /// <value><c>true</c> if this row contains a single Mn; otherwise, <c>false</c>.</value>
        public bool ContainsSingleMn
        {
            get
            {
                return contents.Length == 1 && contents[0] is Mn;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if this <see cref="Mrow"/> contains a single <see cref="Mtable"/>.
        /// </summary>
        public bool ContainsSingleMatrix
        {
            get
            {
                return contents.Length == 3 && contents[1] is Mtable;
            }
        }
        public override void Visit(StringBuilder sb, BuildContext context)
        {
            base.Visit(sb, context);
        }
    }
}