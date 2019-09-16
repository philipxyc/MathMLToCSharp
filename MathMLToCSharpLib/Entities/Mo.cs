﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

namespace MathMLToCSharpLib.Entities
{
    /// <summary>
    /// Operator.
    /// </summary>
    public class Mo : WithTextContent
    {
        private static readonly StringDictionary rep;
        static Mo()
        {
            // replacements in lieu of the actual operator
            rep = new StringDictionary
      {
        {"!", ".Factorial()"},
        {"-", "-"}, // this one is *really* nasty :)
        {"−", "-"}, // this one is also annoying (thanks MathType)
        {"÷", "/"},
        {"×", "*"},
        {"∗", "*"}, // << unpleasant too!
        {"[", "("},
        {"]", ")"},
        {"{", "("},
        {"}", ")"},
        {"⁢" , "*" }, // invisible times unicode character \u2062
        {"\u22c5⁢" , "*" }, // \cdot
        {"⋅" , "*" }, // \cdot
        {",", "."}  // replace the comma with a decimal point
      };
        }

        public Mo() { }
        public Mo(string content) : base(content) { }

        /// <summary>
        /// Returns <c>true</c> if this is an equals operator.
        /// </summary>
        /// <remarks>
        /// Useful for determining whether we are on the RHS or LHS of an assignment.
        /// </remarks>
        public bool IsEquals
        {
            get
            {
                return content == "=";
            }
        }

        /// <summary>
        /// Returns <c>true</c> if this is a closing brace.
        /// </summary>
        public bool IsClosingBrace
        {
            get
            {
                return content == ")";
            }
        }

        /// <summary>
        /// Returns <c>true</c> if this is an opening brace.
        /// </summary>
        public bool IsOpeningBrace
        {
            get
            {
                return content == "(";
            }
        }

        /// <summary>
        /// Returns <c>true</c> if this is a printable operator.
        /// </summary>
        public bool IsPrintableOperator
        {
            get
            {
                return (content != "\u2061" && content != "\u2062" && content != "\u2063");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the content is uppercase sigma.
        /// </summary>
        /// <value><c>true</c> if the content is sigma; otherwise, <c>false</c>.</value>
        public bool IsSigma
        {
            get
            {
                return content == "∑";
            }
        }

        /// <summary>
        /// Gets a value indicating whether the content is uppercase delta.
        /// </summary>
        /// <value><c>true</c> if the content is delta; otherwise, <c>false</c>.</value>
        public bool IsDelta
        {
            get
            {
                return content == "∆";
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is times or star.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is times or star; otherwise, <c>false</c>.
        /// </value>
        public bool IsTimesOrStar
        {
            get
            {
                return "*∗".Contains(content);
            }
        }

        public override void Visit(StringBuilder sb, BuildContext bc)
        {
            bc.Tokens.Add(this);
            //| matrix | detrminate
            if (content == "|")
            {
                if (bc.InMatrixDeterminate)
                {
                    sb.Append(".Determinant()");
                    bc.InMatrixDeterminate = false;
                }
                else
                    bc.InMatrixDeterminate = true;
                return;
            }

            //Built in function
            if (content == "(" && bc.BuiltinFuncPair.Count != 0 && bc.BuiltinFuncPair.Peek().Item2 == false)
            {
                var pr = bc.BuiltinFuncPair.Pop();
                pr = new Tuple<string, bool>(pr.Item1, true);
                bc.BuiltinFuncPair.Push(pr);
                return;
            }
            else if (content == ")" && bc.BuiltinFuncPair.Count != 0 && bc.BuiltinFuncPair.Peek().Item2 == true)
            {
                switch (bc.BuiltinFuncPair.Peek().Item1)
                {
                    default:
                        sb.Append(")");
                        break;
                    case "Eigenvalues":
                        sb.Append(".Evd().EigenValues");
                        break;
                    case "Eigenvectors":
                        sb.Append(".Evd().EigenVectors");
                        break;
                }
                bc.BuiltinFuncPair.Pop();
                return;
            }

            // if we are in subscript mode, adding an operator is not necessary, but neither can we ignore it
            if (bc.Options.SubscriptMode)
            {
                sb.Append("_");
            }
            else if (rep.ContainsKey(content))
                sb.Append(rep[content]);
            else
            {
                Trace.Assert(content.Length == 1, "We only support 1-char operators (for now).");
                if (IsPrintableOperator)
                {
                    if (IsOpeningBrace && bc.LastTokenRequiresTimes)
                        sb.Append("*");

                    sb.Append(content);
                }
            }
        }
    }
}