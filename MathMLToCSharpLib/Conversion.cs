using MathMLToCSharpLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MathMLToCSharpLib
{
    class Conversion
    {
        public Conversion() : this(new BuildContextOptions()) { }
        public Conversion(BuildContextOptions options)
        {
            Singleton.Instance.Options = options;
        }

        public string ParseAndOutput(string mathML)
        {
            IBuildable b = Parser.Parse(XElement.Parse(mathML));
            StringBuilder sb = new StringBuilder();
            b.Visit(sb, new BuildContext(Singleton.Instance.Options));
            return sb.ToString();
        }

        public BuildContextOptions Options
        {
            get { return Singleton.Instance.Options; }
            set { Singleton.Instance.Options = value; }
        }

    }
}
