using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MathMLToCSharpLib.Entities
{
    /// <summary>
    /// This interface must be implemented by all types of expression
    /// objects in the tree.
    /// </summary>
    public interface IBuildable : IXmlSerializable
    {
        /// <summary>
        /// Builds a textual (C#) representation of the model existing at the root of this node.
        /// </summary>
        /// <param name="sb">The builder that aggregates the text.</param>
        /// <param name="bc">Run-time build information.</param>
        void Visit(StringBuilder sb, BuildContext bc);
    }
}