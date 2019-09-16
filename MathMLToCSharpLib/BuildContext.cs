using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using MathMLToCSharpLib.Entities;

namespace MathMLToCSharpLib
{
    /// <summary>
    /// Represents runtime data that is kept while the code is being
    /// assembled from the model.
    /// </summary>
    public sealed class BuildContext
    {

        private readonly IList<String> errors = new List<String>();
        private readonly List<Tuple<ISum, char>> sums = new List<Tuple<ISum, char>>();
        private readonly IList tokens = new ArrayList();
        private readonly ICollection<String> vars = new SortedSet<String>();
        private readonly IList<IBuildable> possibleDivisionsByZero = new List<IBuildable>();
        internal IList<IBuildable> PossibleDivisionsByZero => possibleDivisionsByZero;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildContext"/> class.
        /// </summary>
        public BuildContext() : this(new BuildContextOptions()) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="BuildContext"/> class.
        /// </summary>
        /// <param name="options">Build options.</param>
        public BuildContext(BuildContextOptions options)
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
        public IList<string> Errors => errors;

        /// <summary>
        /// Variables that have been defined during build.
        /// </summary>
        public ICollection<string> Vars => vars;

        /// <summary>
        /// Tokens that have been met during build.
        /// </summary>
        public IList Tokens => tokens;

        /// <summary>
        /// Returns the last token encountered, or <c>null</c> if there are none.
        /// </summary>
        public Object LastToken => tokens.Count > 0 ? tokens[tokens.Count - 1] : null;

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
                if (t is Mo mo)
                {
                    bool isClosing = mo.IsClosingBrace;

                    if (isClosing)
                    {
                        Trace.WriteLine("No * due to closing brace.");
                    }
                    else
                    {
                        Trace.WriteLine("No * as last token is Mo.");
                    }
                    return false;
                }

                switch (t)
                {
                    case WithBinaryContent _:
                        return false;
                    case WithBuildableContents _:
                        //case Mrow _:
                        //case Msqrt _:
                        //case Msup _:
                        //case Msubsup _:
                        return (Tokens.Count > 2
                                && Tokens[Tokens.Count - 2] is Mn
                                );
                    default:
                        Trace.WriteLine("Need *. Last token is " + t.GetType().Name);
                        return true;
                }
            }
        }

        /// <summary>
        /// All variables that have been defined during build.
        /// </summary>
        public IReadOnlyCollection<string> AllVariables => allVariables.AsReadOnlyCollection();

        /// <summary>
        /// Variables on the LHS have been defined during build.
        /// </summary>
        public IReadOnlyCollection<string> LhsVariables => lhsVariables.AsReadOnlyCollection();

        /// <summary>
        /// Variables on the RHS have been defined during build.
        /// </summary>
        public IReadOnlyCollection<string> RhsVariables => rhsVariables.AsReadOnlyCollection();

        private ICollection<string> allVariables { get; } = new SortedSet<string>();

        private ICollection<string> lhsVariables = new HashSet<string>();

        private ICollection<string> rhsVariables { get; } = new HashSet<string>();

        public void AddVariable(string variableName)
        {
            allVariables.Add(variableName);
            if (OnRhs)
            {
                rhsVariables.Add(variableName);
            }
            else
            {
                lhsVariables.Add(variableName);
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
                foreach (object v in tokens)
                {
                    if (v is Mo mo && mo.IsEquals)
                    {
                        return true;
                    }
                }

                return false;
            }
        }


        public IList<Tuple<ISum, char>> Sums => sums;

        public bool InMatrixDeterminate { get; set; }

        //string: function name, bool: go through bracket
        public Stack<Tuple<string, bool>> BuiltinFuncPair = new Stack<Tuple<string, bool>>();

        /// <summary>
        /// Adds a sum.
        /// </summary>
        /// <param name="sum">The sum.</param>
        public void AddSum(ISum sum)
        {
            for (char c = 'a'; c < 'z'; ++c)
            {
                char c1 = c;
                if (!allVariables.Contains(c1.ToString()) &&
                  sums.FindIndex(i => i.Item2 == c1) == -1)
                {
                    sums.Add(new Tuple<ISum, char>(sum, c));
                    allVariables.Add(c.ToString());
                    return;
                }
            }
            // todo: make this more civil later on
            throw new Exception("Out of variables!");
        }

        public char GetSumIdentifier(ISum sum)
        {
            int idx = sums.FindIndex(i => i.Item1 == sum);
            if (idx != -1)
                return sums[idx].Item2;
            else return '?';
        }
    }

    //internal class BuildContextSum
    //{
    //    private IBuildable[] initTokens;
    //    private IBuildable[] limitTokens;
    //    private IBuildable[] statement;
    //}

    public enum EquationDataType
    {
        Float,
        Double,
        Decimal
    }
}
