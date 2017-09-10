using System;
using System.Diagnostics;
using System.Text;

namespace Tests
{
  public class Program
  {
    private static void Main(string[] args)
    {
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
      a = 0.42748 * ((Math.Pow((R*T_c), 2)) / (P_c)) * 
        Math.Pow((1 + m * (1 - Math.Sqrt(T_r))), 2);
    }
  }
}