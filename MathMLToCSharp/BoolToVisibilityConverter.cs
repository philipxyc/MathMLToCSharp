using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MathMLToCSharp
{
  [ValueConversion(typeof(bool), typeof(Visibility))]
  public class BoolToVisibilityConverter : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType,
      object parameter, CultureInfo culture)
    {
      return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type targetType,
      object parameter, CultureInfo culture)
    {
      return false;
    }

    #endregion
  }
}
