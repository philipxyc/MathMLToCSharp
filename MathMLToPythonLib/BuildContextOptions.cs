using System;
using System.ComponentModel;

namespace MathMLToPythonLib
{
  /// <summary>
  /// Options used for building the code.
  /// </summary>
  [Serializable]
  public sealed class BuildContextOptions : INotifyPropertyChanged
  {
    private bool deltaPartOfIdent = true;
    private EquationDataType eqnDataType = EquationDataType.Double;
    private bool greekToRoman;
    private int maxInlinePower = 2;
    private bool numberPostfix;
    private bool parallelize;
    private bool reduceFractions = true;
    private bool replaceEWithMathE;
    private bool replaceExpWithMathExp = true;
    private bool replacePiWithMathPI;
    private bool singleLetterVars = true;
    private bool treatSigmaAsSum = true;

    public BuildContextOptions()
    {
      ReplacePiWithMathPI = true;
    }

    /// <summary>
    /// When set, prevents the builder from adding variables or appending * in front of them.
    /// Also, superscripts are treated as ordinary <c>Mn</c> elements when this is set.
    /// </summary>
    public bool SubscriptMode { get; set; }

    [Category("Substitution")]
    [DisplayName("Replace exp with Math.Exp")]
    [Description("Instances of exp(x) are replaced with Math.Exp(x).")]
    public bool ReplaceExpWithMathExp
    {
      get
      {
        return replaceExpWithMathExp;
      }
      set
      {
        replaceExpWithMathExp = value;
        NotifyPropertyChanged("ReplaceExpWithMathExp");
      }
    }

    [Category("Substitution")]
    [DisplayName("Max Inline Power")]
    public int MaxInlinePower
    {
      get
      {
        return maxInlinePower;
      }
      set
      {
        maxInlinePower = value;
        NotifyPropertyChanged("MaxInlinePower");
      }
    }

    [Category("Substitution")]
    [DisplayName("Replace π with Math.PI")]
    [Description("Instances of Math.Pow(n, 2) are replaced with n*n.")]
    public bool ReplacePiWithMathPI
    {
      get { return replacePiWithMathPI; }
      set
      {
        replacePiWithMathPI = value;
        NotifyPropertyChanged("ReplacePiWithMathPI");
      }
    }

    [Category("Substitution")]
    [DisplayName("Replace e with Math.E")]
    public bool ReplaceEWithMathE
    {
      get { return replaceEWithMathE; }
      set
      {
        replaceEWithMathE = value;
        NotifyPropertyChanged("ReplaceeWithMathE");
      }
    }

    [DisplayName("Single-letter variables")]
    [Category("Keep Options")]
    [Description("When set, multi-letter variables (e.g., 'abc') will be split into single-letter ones (e.g., 'a', 'b', 'c').")]
    public bool SingleLetterVars
    {
      get { return singleLetterVars; }
      set
      {
        singleLetterVars = value;
        NotifyPropertyChanged("SingleLetterVars");
      }
    }

    [Category("Substitution")]
    [DisplayName("Greek to Roman")]
    [Description("When set, Greek identifiers will be replaced with their romanized names.")]
    public bool GreekToRoman
    {
      get { return greekToRoman; }
      set
      {
        greekToRoman = value;
        NotifyPropertyChanged("GreekToRoman");
      }
    }

    [DisplayName("Data type")]
    [Description("The default data type used by the code generator")]
    public EquationDataType EqnDataType
    {
      get { return eqnDataType; }
      set
      {
        eqnDataType = value;
        NotifyPropertyChanged("EqnDataType");
      }
    }

    [DisplayName("Number postfix")]
    [Description("When set, all numbers will have a postfix corresponding to their data type.")]
    public bool NumberPostfix
    {
      get { return numberPostfix; }
      set
      {
        numberPostfix = value;
        NotifyPropertyChanged("NumberPostfix");
      }
    }

    [DisplayName("Keep Δ attached")]
    [Category("Keep Options")]
    [Description("When set, the letter Δ will be stuck to the letter that follows it (if any).")]
    public bool DeltaPartOfIdent
    {
      get { return deltaPartOfIdent; }
      set
      {
        deltaPartOfIdent = value;
        NotifyPropertyChanged("DeltaPartOfIdent");
      }
    }

    [Category("Substitution")]
    [DisplayName("Treat Σ as sum")]
    [Description("When set, the letter Σ will be treated as a summation operator, and appropriate code will be emitted.")]
    public bool TreatSigmaAsSum
    {
      get { return treatSigmaAsSum; }
      set
      {
        treatSigmaAsSum = value;
        NotifyPropertyChanged("TreatSigmaAsSum");
      }
    }

    [Description("When set, causes all loops to be defined using Parallel Extensions.")]
    public bool Parallelize
    {
      get
      {
        return parallelize;
      }
      set
      {
        parallelize = value;
        NotifyPropertyChanged("Parallelize");
      }
    }

    [Category("Substitution")]
    [DisplayName("Reduce Fractions")]
    [Description("When set, simple fractions (e.g., 1/2) are substituted by their result (e.g., 0.5).")]
    public bool ReduceFractions
    {
      get { return reduceFractions; }
      set
      {
        reduceFractions = value;
        NotifyPropertyChanged("ReduceFractions");
      }
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    private void NotifyPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this,
                        new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}