using System;
using System.Windows;
using System.Windows.Threading;
using MathMLToCSharp.UI.Properties;

namespace MathMLToCSharp
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    /// <summary>
    /// Gets or sets the build options.
    /// </summary>
    /// <value>The build options.</value>
    internal static BuildContextOptions BuildOptions { get; set; }

    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      MessageBox.Show(
        e.Exception.Message,
        "Unhandled exception",
        MessageBoxButton.OK,
        MessageBoxImage.Error);
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
      // get the build options if they exist
      if (Settings.Default["BuildOptions"] == null)
        BuildOptions = new BuildContextOptions();
      else
        BuildOptions = Settings.Default["BuildOptions"] as BuildContextOptions;


      // todo: apply theme
      try
      {
        Uri uri = new Uri(
          "PresentationFramework.Royale;V3.0.0.0;31bf3856ad364e35;component\\themes/Royale.normalcolor.xaml",
          UriKind.Relative);
        Resources.MergedDictionaries.Add(LoadComponent(uri) as ResourceDictionary);
      } catch {}
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
      Settings.Default["BuildOptions"] = BuildOptions;
      Settings.Default.Save();
    }
  }
}
