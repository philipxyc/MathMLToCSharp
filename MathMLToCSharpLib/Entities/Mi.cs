using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Wintellect.PowerCollections;

namespace MathMLToCSharpLib.Entities
{
    /// <summary>
    /// Identifier (a.k.a. variable).
    /// </summary>
    public class Mi : WithTextContent
    {
        public Mi(string content) : base(content) { }

        /// <summary>
        /// Returns <c>true</c> if this is a function not supported by System.Math.
        /// </summary>
        public bool IsUnsupportedFunction
        {
            get
            {
                return content == "sec" || content == "sech" ||
                       content == "csc" || content == "csch" ||
                       content == "cot" || content == "coth";
            }
        }

        public override void Visit(StringBuilder sb, BuildContext bc)
        {
            if (string.IsNullOrEmpty(content))
                return;

            //postfix Built-in function detection
            if (content == "Eigenvectors" || content == "Eigenvalues")
            {
                bc.BuiltinFuncPair.Push(new Pair<string, bool>(content, false));
                bc.Tokens.Add(this);
                return;
            }

            // get every single variable, unless this is a function name rather than a variable name
            // this assumes that nobody intends to call a variable, e.g., 'sin'
            // also, the variable is not converted if it starts with a delta and we've chosen to
            // keep delta as part of identifier
            List<string> vars = (bc.Options.SingleLetterVars && !Semantics.rep.ContainsKey(content) &&
                                 !(bc.Options.DeltaPartOfIdent && content[0] == '∆'))
                              ?
                                new List<string>(Array.ConvertAll(content.ToCharArray(), c => c.ToString()))
                              :
                                new List<string> { content };

            for (int i = 0; i < vars.Count; i++)
            {
                string varName = vars[i];

                bool needReplace = false;
                string replaceTerm = string.Empty;
                if (Semantics.rep.ContainsKey(varName))
                {
                    Pair<string, string> p = Semantics.rep[varName];
                    replaceTerm = p.First;
                    if (string.IsNullOrEmpty(p.Second))
                        needReplace = true;
                    else
                    {
                        Type bct = typeof(BuildContextOptions);
                        PropertyInfo pi = bct.GetProperty(p.Second);
                        Debug.Assert(pi != null);
                        needReplace = (bool)pi.GetValue(bc.Options, null);
                    }
                }

                // if variable not subject to replacement, check to see if it requires conversion from greek
                if (!needReplace && varName.Length == 1 && bc.Options.GreekToRoman)
                {
                    varName = Util.GreekLetterName(varName[0]);
                }

                // can only declare variable if
                // 1) if it hasn't already been declared
                // 2) if there needn't be a replacement, e.g., of e with Math.E
                // 3) it hasn't been explicitly blocked
                if (!bc.Vars.Contains(varName) && !needReplace && !bc.Options.SubscriptMode)
                    bc.Vars.Add(varName);

                // use implicit multiplication if necessary
                if (bc.LastTokenRequiresTimes && !bc.Options.SubscriptMode)
                    sb.Append("*");
                // add type to context
                if (needReplace)
                {
                    sb.Append(replaceTerm);
                }
                else
                {
                    sb.Append(varName);
                }
                // unless this is the last variable, add multiplication sign after it
                if (i + 1 != vars.Count)
                    sb.Append("*");
            }

            bc.Tokens.Add(this);
        }
    }
}