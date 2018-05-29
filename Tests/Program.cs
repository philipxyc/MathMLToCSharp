using MathMLToCSharpLib.Entities;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Tests
{
    public class Program
    {
        private static void Main(string[] args)
        {

            MathMLToCSharpLib.Entities.Math math = new MathMLToCSharpLib.Entities.Math(new IBuildable[]
                {
                    new Mrow(new IBuildable[]
                    {
                        new Mi("x"),
                        new Mo("+"),
                        new Msup(new Mi("y"), new Mrow(new IBuildable[]
                        {
                            new Mfrac(new Mn("2"), new Mrow(new IBuildable[]
                            {
                                new Mi("k"),
                                new Mo("+"),
                                new Mn("1")
                            }))
                        }))
                    })
                });

            /* Write XML */
            StringBuilder sb2 = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb2);
            
            math.WriteXml(writer);
            writer.Close();
            //XElement xElement = math.ToXElement();

            /* Read XML */
            XmlTextReader reader = new XmlTextReader(new StringReader(sb2.ToString()));
            MathMLToCSharpLib.Entities.Math math2 = new MathMLToCSharpLib.Entities.Math("");
            math2.ReadXml(reader);

            StringBuilder sb3 = new StringBuilder();
            XmlWriter writer2 = XmlWriter.Create(sb3);
            math2.WriteXml(writer2);
            writer2.Close();

            /* Check if Read/Write method produces the same xml */
            bool areEqual = sb2.ToString() == sb3.ToString();

            string s = "(*(";
            s = s.Replace("(*", "(");
            Console.WriteLine(s);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("(*((");
            sb.Replace("(*", "(");
            Console.WriteLine(sb);


            double a = 0.0;
            double m = 0.0;
            double P_c = 0.0;
            double R = 0.0;
            double T_c = 0.0;
            double T_r = 0.0;
            Debug.Assert(P_c != 0);
            a = 0.42748 * ((System.Math.Pow((R * T_c), 2)) / (P_c)) *
              System.Math.Pow((1 + m * (1 - System.Math.Sqrt(T_r))), 2);
        }
    }
}