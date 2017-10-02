using MathMLToPythonLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MathMLToPythonLib
{
    public class Conversion
    {
        public Conversion() : this(new BuildContextOptions()) { }
        public Conversion(BuildContextOptions options)
        {
            Singleton.Instance.Options = options;
        }

        public string ParseAndOutput(string mathML,out bool multiLine)
        {
            IBuildable b = Parser.Parse(XElement.Parse(mathML));
            StringBuilder sb = new StringBuilder();
            b.Visit(sb, new BuildContext(Singleton.Instance.Options));
            multiLine = Singleton.Instance.isMultiLine;
            Singleton.Instance.isMultiLine = false;// back to initial
            return sb.ToString();
        }
        public string ParseAndOutput(string mathML)
        {
            IBuildable b = Parser.Parse(XElement.Parse(mathML));
            StringBuilder sb = new StringBuilder();
            b.Visit(sb, new BuildContext(Singleton.Instance.Options));
            Singleton.Instance.isMultiLine = false;// back to initial
            return sb.ToString();
        }

        public BuildContextOptions Options
        {
            get { return Singleton.Instance.Options; }
            set { Singleton.Instance.Options = value; }
        }

    }
}
