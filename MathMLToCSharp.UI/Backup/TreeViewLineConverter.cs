using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace MathMLToCSharp
{
  class TreeViewLineConverter : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      TreeViewItem item = (TreeViewItem)value;
      ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
      return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion
  }
}
