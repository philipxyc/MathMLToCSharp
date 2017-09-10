using System;
using System.ComponentModel;
using System.Windows;
using NUnit.Framework;
using System.Diagnostics;
using System.Xml.Linq;
namespace Tests
{
  public class MockObject : DependencyObject
  {
    [DisplayName("Is enabled")]
    public bool IsEnabled { get; set; }

    public static readonly DependencyProperty NameProperty =
      DependencyProperty.Register("Name", typeof(string), typeof(MockObject), new PropertyMetadata("Unknown"));

    [DisplayName("Some name or other")]
    public string Name
    {
      get { return (string)GetValue(NameProperty); }
      set { SetValue(NameProperty, value); }
    }
  }

  [TestFixture]
  public class OtherTests
  {
    [Test]
    public void DisplayNameTest()
    {
      PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(typeof (MockObject),
         new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.SetValues | 
                                                      PropertyFilterOptions.UnsetValues | 
                                                      PropertyFilterOptions.Valid) });
      foreach (PropertyDescriptor pd in pdc)
      {
        Trace.WriteLine(pd.Name);
        if (pd.Name == "IsEnabled")
          Assert.AreEqual("Is enabled", pd.DisplayName);
        if (pd.Name == "Name")
          Assert.AreEqual("Some name or other", pd.DisplayName);
        
        
      }
    }

    [Test]
    public void XElementBehaviorTest()
    {
      XElement root = XElement.Parse("<root></root>");
      root.Name = "other";
      Trace.WriteLine(root.ToString());
    }
  }
}
