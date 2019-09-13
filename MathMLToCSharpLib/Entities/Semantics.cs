using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MathMLToCSharpLib.Entities
{
    /// <summary>
    /// Completely spurious element that MathType 6 insists on generating.
    /// </summary>
    public class Semantics : WithBuildableContents
    {
        internal const string starPrefix = "_star";

        /// <summary>
        /// Functions which need replacement.
        /// </summary>
        internal static readonly List<string> knownFuncts;

        /// <summary>
        /// Symbols and function names that can be replaced.
        /// </summary>
        internal static readonly Dictionary<string, Tuple<string, string>> rep;
        /// <summary>
        /// Trig functions which have an inverse, and their inverse values.
        /// </summary>
        internal static StringDictionary inverseTrigs;


        static Semantics()
        {
            rep = new Dictionary<string, Tuple<string, string>>
      {
        {"∞", new Tuple<string, string>("double.MaxValue", null)},
        {"π", new Tuple<string, string>("Math.PI", "ReplacePiWithMathPI")},
        {"e", new Tuple<string, string>("Math.E", "ReplaceEWithMathE")},
        {"exp", new Tuple<string,string>("Math.Exp", "ReplaceExpWithMathExp")},
        {"*", new Tuple<string,string>("star", null)},

        // functions
        {"cos",  new Tuple<string, string>("Math.Cos(", null)},
        {"cosh", new Tuple<string, string>("Math.Cosh(", null)},
        {"sec",  new Tuple<string, string>("1/Math.Cos(", null)},
        {"sech", new Tuple<string, string>("1/Math.Cosh(", null)},
        {"sin",  new Tuple<string, string>("Math.Sin(", null)},
        {"sinh", new Tuple<string, string>("Math.Sinh(", null)},
        {"csc",  new Tuple<string, string>("1/Math.Sin(", null)},
        {"csch", new Tuple<string, string>("1/Math.Sinh(", null)},
        {"tan",  new Tuple<string, string>("Math.Tan(", null)},
        {"tanh", new Tuple<string, string>("Math.Tanh(", null)},
        {"cot",  new Tuple<string, string>("1/Math.Tan(", null)},
        {"coth", new Tuple<string, string>("1/Math.Tanh(", null)},
        {"ln",   new Tuple<string, string>("Math.Log(", null)},
        {"log",  new Tuple<string, string>("Math.Log10(", null)},

        // inverse trig functions
        {"arcsin", new Tuple<string,string>("Math.Asin(", null)},
        {"arccos", new Tuple<string,string>("Math.Acos(", null)},
        {"arctan", new Tuple<string,string>("Math.Atan(", null)},
        {"arccsc", new Tuple<string,string>("Math.Asin(1/", null)},
        {"arcsec", new Tuple<string,string>("Math.Acos(1/", null)},
        {"arccot", new Tuple<string,string>("Math.Atan(1/", null)}
      };

            inverseTrigs = new StringDictionary
      {
        {"sin", "arcsin"},
        {"cos", "arccos"},
        {"tan", "arctan"},
        {"csc", "arccsc"},
        {"sec", "arcsec"},
        {"cot", "arccot"}
      };

            knownFuncts = new List<string>
      {
        "cos",
        "cosh",
        "arcsin",
        "sec",
        "sech",
        "arcsec",
        "sin",
        "sinh",
        "arcsin",
        "csc",
        "csch",
        "arccsc",
        "tan",
        "tanh",
        "arctan",
        "cot",
        "coth",
        "arccot",
        "ln",
        "log",
        "exp" // exponent, should be user-selectable
      };
        }

        public Semantics(IBuildable[] contents) : base(contents) { }

        internal static string postfixForDataType(EquationDataType type)
        {
            switch (type)
            {
                case EquationDataType.Decimal:
                    return "M";
                case EquationDataType.Float:
                    return "f";
                default:
                    return string.Empty;
            }
        }
    }
}