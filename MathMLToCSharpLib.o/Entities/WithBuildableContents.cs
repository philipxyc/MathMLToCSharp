using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Wintellect.PowerCollections;

namespace MathMLToCSharpLib.Entities
{
    public abstract class WithBuildableContents : IBuildable
    {
        public IBuildable[] contents;
        protected WithBuildableContents()
        {
        }

        protected WithBuildableContents(IBuildable[] contents)
        {
            this.contents = contents;

            ReplaceInverseTrigFunctions(contents);
        }

        public IBuildable LastElement
        {
            get
            {
                if (contents.Length > 0)
                    return contents[contents.Length - 1];
                return null;
            }
        }

        #region IBuildable Members

        public virtual void Visit(StringBuilder sb, BuildContext context)
        {
            bool containsFunct = false;
            int numFuncts = 0;

            if (sb == null || context == null || contents == null || contents.Length == 0)
                return;

            // copy contents into list
            List<IBuildable> ctxCopy = new List<IBuildable>(contents);

            // check if the Sigma operator acts as a sum
            if (context.Options.TreatSigmaAsSum)
            {
                // look for the plain sigma operator
                int index;
                while ((index = ctxCopy.FindIndex(a => (a is Mo && (a as Mo).IsSigma))) != -1)
                {
                    // so long as it is not the last element
                    if (index != ctxCopy.Count - 1)
                    {
                        BuildablePlainSum bps = new BuildablePlainSum(ctxCopy[index + 1]);
                        ctxCopy.RemoveAt(index + 1);
                        ctxCopy[index] = bps;

                        context.AddSum(bps);
                    }
                }
            }

            // check if delta <mo> appears before an <mi>
            if (context.Options.DeltaPartOfIdent)
            {
                int index;
                while ((index = ctxCopy.FindIndex(a => (a is Mo && (a as Mo).IsDelta))) != -1)
                {
                    // check that it's not the last element
                    if (index != ctxCopy.Count - 1)
                    {
                        // check that the next element is <mi>
                        Mi mi = ctxCopy[index + 1] as Mi;
                        if (mi != null)
                        {
                            // change Mi's content to incorporate the delta
                            Mi newMi = new Mi("∆" + mi.Content);
                            ctxCopy[index + 1] = newMi;
                            // remove the delta
                            ctxCopy.RemoveAt(index);
                            Trace.WriteLine(newMi.Content);
                        }
                    }
                }
            }
            else
            {
                // change delta from mo to mi
                int index;
                while ((index = ctxCopy.FindIndex(a => (a is Mo && (a as Mo).IsDelta))) != -1)
                {
                    ctxCopy[index] = new Mi("∆");
                }
            }

            // Scan for functions.
            // If one is found, increment the number. Because nested
            // functions which do not contain parentheses (e.g. sin sin x)
            // are contained in the same row element (in non-Word generated markup),
            // this allows us to determine how many close parens we need.
            foreach (IBuildable v in ctxCopy)
            {
                if (v is Mi && Semantics.knownFuncts.Contains((v as Mi).Content))
                {
                    containsFunct = true;
                    numFuncts++;
                }
            }

            if (containsFunct)
            {
                // Only process if this is not the spurious function.
                if (ctxCopy.Count != 1 || (!Semantics.knownFuncts.Contains((ctxCopy[0] as Mi).Content)))
                {
                    foreach (IBuildable v in ctxCopy)
                        v.Visit(sb, context);

                    for (; numFuncts > 0; numFuncts--)
                        sb.Append(")");
                }
            }
            // No function was found, so process in the normal fashion.
            else
            {
                foreach (var v in ctxCopy)
                    v.Visit(sb, context);
            }
        }

        #region IXmlSerializable Members
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            List<IBuildable> tempContent = new List<IBuildable>();
            while(reader.Read())
            {
                if (reader.IsStartElement())
                {
                    Type type = Type.GetType(this.GetType().Namespace + "." + reader.Name);
                    IBuildable element = (IBuildable)Activator.CreateInstance(type);
                    tempContent.Add(element);
                    element.ReadXml(reader);
                }
                else if (reader.NodeType == XmlNodeType.EndElement & reader.Name == this.GetType().Name)
                {
                    contents = tempContent.ToArray();
                    return;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(this.GetType().Name);

            foreach (IBuildable item in contents)
                item.WriteXml(writer);

            writer.WriteEndElement();
        }
        #endregion
        #endregion

        /// <summary>
        /// This function replaces all known inverse trig functions (in the msup blocks)
        /// by inverse names, so that sin^-1 becomes arcsin.
        /// </summary>
        /// <param name="contents">Buidlable contents</param>
        private static void ReplaceInverseTrigFunctions(IBuildable[] contents)
        {
            for (int i = 0; i < contents.Length; i++)
            {
                IBuildable c = contents[i];
                string trigFunction = string.Empty;
                if (c is Msup)
                {
                    bool funcIsTrig = false, radixIsNeg1 = false;
                    Pair<IBuildable, IBuildable> terms = (c as Msup).Values;
                    if (terms.First is Mrow)
                    {
                        Mrow row1 = terms.First as Mrow;
                        if (row1.Contents.Length > 0 && row1.Contents[0] is Mi)
                        {
                            if (Semantics.inverseTrigs.ContainsKey((row1.Contents[0] as Mi).Content))
                            {
                                trigFunction = (row1.Contents[0] as Mi).Content;
                                funcIsTrig = true;
                            }
                        }
                    }
                    if (terms.Second is Mrow)
                    {
                        Mrow row2 = terms.Second as Mrow;
                        StringBuilder sb = new StringBuilder();
                        BuildContext bc = new BuildContext();
                        row2.Visit(sb, bc);
                        if (sb.ToString() == "-1")
                            radixIsNeg1 = true;
                    }
                    // if this is an inverse function, replace an <msup> with an inverse <mi>
                    if (funcIsTrig && radixIsNeg1)
                    {
                        Mi mi = new Mi(Semantics.inverseTrigs[trigFunction]);
                        contents[i] = mi;
                    }
                }
            }
        }
    }
}