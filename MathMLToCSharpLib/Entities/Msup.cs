using System;
using System.Collections.Generic;
using System.Text;

namespace MathMLToCSharpLib.Entities
{
    /// <summary>
    /// Superscript. Taken to mean 'power of' unless the superscript parses into a *.
    /// </summary>
    public class Msup : WithBinaryContent
    {
        public Msup() { }
        public Msup(IBuildable first, IBuildable second) : base(first, second) { }

        public Tuple<IBuildable, IBuildable> Values
        {
            get
            {
                return new Tuple<IBuildable, IBuildable>(first, second);
            }
        }

        public override void Visit(StringBuilder sb, BuildContext bc)
        {
            if (bc.Options.SubscriptMode)
            {
                // separate by double underscore to differentiate from Msub and prevent clashes
                first.Visit(sb, bc);
                sb.Append("__");
                second.Visit(sb, bc);
                bc.Tokens.Add(this);
            }
            else if (second is Mo mo && mo.IsTimesOrStar && first is Mi mi)
            {
                // this isn't really a superscript - it's part of the variable
                Mi newMi = new Mi(mi.Content + Semantics.StarPrefix);
                newMi.Visit(sb, bc);
                bc.Tokens.Add(this);
            }
            else
            {
                if ((first is Mrow mrow) && (mrow.ContainsSingleMtable))
                {
                    if (second is Mrow mrow2)
                    {
                        if (mrow2.ContainsSingleMi && (mrow2.LastElement as Mi)?.Content == "T")
                        {
                            mrow.Visit(sb, bc);
                            sb.Append(".Transpose()");
                        }
                        else if (mrow2.ContainsSingleMn && (mrow2.LastElement as Mn).IsIntegerGreaterThan1)
                        {
                            mrow.Visit(sb, bc);
                            sb.Append(".Power(");
                            second.Visit(sb, bc);
                            sb.Append(")");
                        }
                        else if (mrow2.ContainsSingleMn && (mrow2.LastElement as Mn).Content == "-1")
                        {
                            mrow.Visit(sb, bc);
                            sb.Append(".Inverse()");
                        }
                    }

                    bc.Tokens.Add(this);
                    return;
                }

                if (bc.LastTokenRequiresTimes)
                    sb.Append("*");
                bc.Tokens.Add(this);

                // determine whether power must be inlined
                bool secondIsIntegralPower = (second is Mrow second1) && (second1.ContainsSingleMn) &&
                                             (second1.LastElement as Mn).IsIntegerGreaterThan1;
                bool mustInlinePower = false;
                int power = 0;
                if (secondIsIntegralPower)
                {
                    // casting is okay as the test for boolean has already done the checks
                    power = ((Mn)((Mrow)second).LastElement).IntegerValue;
                    mustInlinePower = power <= bc.Options.MaxInlinePower;
                }
                if (mustInlinePower)
                {
                    bool firstIsMrowMn = (first is Mrow mrow1) && (mrow1.ContainsSingleMn);
                    for (int i = 0; i < power; ++i)
                    {
                        if (i != 0 && firstIsMrowMn)
                            sb.Append("*"); //for the case mn^2 not appended * automatically
                        first.Visit(sb, bc); // * sign appended automatically
                    }
                }
                else
                {
                    sb.Append("Math.Pow(");
                    first.Visit(sb, bc);
                    sb.Append(", ");
                    second.Visit(sb, bc);
                    sb.Append(")");
                }
            }

        }
    }
}