using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MathMLToCSharp.Entities;
using Wintellect.PowerCollections;

namespace MathMLToCSharp
{
    /// <summary>
    /// Represents runtime data that is kept while the code is being
    /// assembled from the model.
    /// </summary>
    public sealed class BuildContext
    {
        private readonly IList<String> errors = new List<String>();
        private readonly List<Pair<ISum, char>> sums = new List<Pair<ISum, char>>();
        private readonly IList tokens = new ArrayList();
        private readonly ICollection<String> vars = new OrderedSet<String>();
        private readonly IList<IBuildable> possibleDivisionsByZero = new List<IBuildable>();
        internal IList<IBuildable> PossibleDivisionsByZero
        {
            get
            {
                return possibleDivisionsByZero;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildContext"/> class.
        /// </summary>
        public BuildContext() : this(new BuildContextOptions()) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="BuildContext"/> class.
        /// </summary>
        /// <param name="options">Build options.</param>
        internal BuildContext(BuildContextOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// Options used for building the code.
        /// </summary>
        internal BuildContextOptions Options { get; private set; }

        /// <summary>
        /// Errors encountered during build.
        /// </summary>
        public IList<string> Errors
        {
            get { return errors; }
        }

        /// <summary>
        /// Variables that have been defined during build.
        /// </summary>
        public ICollection<string> Vars
        {
            get { return vars; }
        }

        /// <summary>
        /// Tokens that have been met during build.
        /// </summary>
        public IList Tokens
        {
            get { return tokens; }
        }

        /// <summary>
        /// Returns the last token encountered, or <c>null</c> if there are none.
        /// </summary>
        public Object LastToken
        {
            get
            {
                return tokens.Count > 0 ? tokens[tokens.Count - 1] : null;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if last token suggests the use of times before identifier.
        /// </summary>
        public bool LastTokenRequiresTimes
        {
            get
            {
                if (LastToken == null) return false;

                // times is required in all cases, except when
                // - last token was an operator and not a closing brace
                object t = LastToken;
                bool isMo = t is Mo;
                bool isClosing = false;
                if (isMo)
                    isClosing = ((Mo)t).IsClosingBrace;

                if (isClosing)
                {
                    Trace.WriteLine("No * due to closing brace.");
                    return true;
                }
                if (isMo)
                {
                    Trace.WriteLine("No * as last token is Mo.");
                    return false;
                }

                if (t is Msup | t is Mrow) return false;

                Trace.WriteLine("Need *. Last token is " + t.GetType().Name);
                return true;
            }
        }

        /// <summary>
        /// Indicates whether we are on the right-hand side (after =) of the equation.
        /// </summary>
        public bool OnRhs
        {
            get
            {
                // note: completely wrong. = can appear in, e.g., sum subscripts
                foreach (var v in tokens)
                    if (v is Mo && (v as Mo).IsEquals)
                        return true;
                return false;
            }
        }


        public IList<Pair<ISum, char>> Sums
        {
            get
            {
                return sums;
            }
        }
        public bool InMatrixDeterminate { get; set; }

        //string: function name, bool: go through bracket
        public Stack<Pair<string,bool>> BuiltinFuncPair = new Stack<Pair<string, bool>>();

        /// <summary>
        /// Adds a sum.
        /// </summary>
        /// <param name="sum">The sum.</param>
        public void AddSum(ISum sum)
        {
            for (char c = 'a'; c < 'z'; ++c)
            {
                char c1 = c;
                if (!vars.Contains(c1.ToString()) &&
                  sums.FindIndex(i => i.Second == c1) == -1)
                {
                    sums.Add(new Pair<ISum, char>(sum, c));
                    vars.Add(c.ToString());
                    return;
                }
            }
            // todo: make this more civil later on
            throw new Exception("Out of variables!");
        }

        public char GetSumIdentifier(ISum sum)
        {
            int idx = sums.FindIndex(i => i.First == sum);
            if (idx != -1)
                return sums[idx].Second;
            else return '?';
        }
    }

    internal class BuildContextSum
    {
        private IBuildable[] initTokens;
        private IBuildable[] limitTokens;
        private IBuildable[] statement;
    }

    public enum EquationDataType
    {
        Float,
        Double,
        Decimal
    }
}
