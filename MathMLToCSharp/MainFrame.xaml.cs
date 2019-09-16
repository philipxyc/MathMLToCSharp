using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

using MathMLToCSharp.Properties;

using MathMLToCSharpLib;

using Microsoft.CSharp;

namespace MathMLToCSharp
{
    /// <summary>
    /// Interaction logic for MainFrame.xaml
    /// </summary>
    public partial class MainFrame : Window
    {
        // Using a DependencyProperty as the backing store for ShowTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowTreeProperty =
            DependencyProperty.Register("ShowTree", typeof(bool), typeof(MainFrame), new UIPropertyMetadata(false));

        public MainFrame()
        {
            InitializeComponent();
            WindowState = (WindowState)Settings.Default["WindowState"];

            Version version = Assembly.GetAssembly(GetType()).GetName().Version;
            Title += string.Format(" v.{0}.{1}.{2}", version.Major, version.Minor, version.Build);
#if DEBUG
            Title += " DEBUG";
            ShowTree = true;
#endif
            propGrid.SelectedObject = App.BuildOptions;
            App.BuildOptions.PropertyChanged += delegate { tbIn_TextChanged(this, null); };
        }

        /// <summary>
        /// When set, shows the MathML tree.
        /// </summary>
        public bool ShowTree
        {
            get { return (bool)GetValue(ShowTreeProperty); }
            set { SetValue(ShowTreeProperty, value); }
        }

        private void tbIn_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbOut.Text = string.Empty;
            // try parsing the root
            XElement root;
            try
            {
                string str = tbIn.Text;
                root = XElement.Parse(str);
            }
            catch (Exception ex)
            {
                tbOut.Text = ex.Message;
                return;
            }
            // fill the tree with xml elements
            FillTree(root);

            // now set output content
#if RELEASE
        try
        {
#endif
            var m = Parser.Parse(root);
            var sb = new StringBuilder();
            m.Visit(sb, new BuildContext(App.BuildOptions));
            string s = sb.ToString();
            bool useHacks = true;

            if (useHacks)
            {
                // here's what happens when you realize string.Replace doesn't work as it should
                s = s.Replace("**", "*");
                s = s.Replace("(*", "(");
                s = s.Replace(")(", ")*(");
                s = s.Replace(")Math", ")*Math");
                s = s.Replace("+*", "+");
                s = s.Replace("-*", "-");
                s = s.Replace("/*", "/");

                // hack: even nastier hack here
                Regex re = new Regex(@"(\d)(\()");
                foreach (Match match in re.Matches(sb.ToString()))
                {
                    string toReplace = match.Value;
                    string toReplaceWith = match.Groups[1].Value + "*" + match.Groups[2].Value;
                    s = s.Replace(toReplace, toReplaceWith);
                }
            }

            tbOut.Text = s;
#if RELEASE
        }
        catch (Exception ex)
        {
          tbOut.Text = ex.Message +
                       Environment.NewLine +
                       ex.StackTrace;
        }
#endif
        }


        private void FillTree(XElement root)
        {
            if (root == null) throw new ArgumentNullException("root");
            // remove all items
            mmlTree.Items.Clear();
            // add root
            var rootItem = new TreeViewItem
            {
                Header = root.Name.LocalName,
                IsExpanded = true
            };
            mmlTree.Items.Add(rootItem);

            // add other items recursively
            AddTreeNodes(rootItem, root.Elements());
        }

        private static void AddTreeNodes(ItemsControl rootItem, IEnumerable<XElement> elems)
        {
            if (rootItem == null) throw new ArgumentNullException("rootItem");
            if (elems == null) throw new ArgumentNullException("elems");
            foreach (var elem in elems)
            {
                var thisItem = new TreeViewItem
                {
                    Header = elem.Name.LocalName +
                    (elem.HasElements ? "" : " [" +
                      (elem.Value.Length > 20 ? string.Empty : elem.Value)
                    + "]"),
                    IsExpanded = true
                };

                // if the type for this node does not exist in our wonderful little app,
                // make it bold and red!
                try
                {
                    Type t = Type.GetType("MathMLToCSharp.Entities." + elem.Name.LocalName);
                    if (t == null)
                        throw new NullReferenceException();
                    thisItem.Foreground = Brushes.Black;
                    thisItem.FontWeight = FontWeights.Normal;
                }
                catch
                {
                    thisItem.Foreground = Brushes.Red;
                    thisItem.FontWeight = FontWeights.Bold;
                }

                rootItem.Items.Add(thisItem);
                AddTreeNodes(thisItem, elem.Elements());
            }
        }

        private void lnkCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(tbOut.Text);
        }

        private void lnkPaste_Click(object sender, RoutedEventArgs e)
        {
            tbIn.Text = Clipboard.GetText();
        }

        private void myWindow_Closing(object sender, CancelEventArgs e)
        {
            Settings.Default["WindowState"] = WindowState;
        }

        private void lnkFormat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XElement xe = XElement.Parse(tbIn.Text);
                tbIn.Text = xe.ToString();
            }
            catch (Exception ex)
            {
                tbOut.Text = ex.Message;
            }
        }

        private void myWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // set window to fixed size for screenshots
#if DEBUG
            if (e.Key == Key.F12)
            {
                Width = 640;
                Height = 480;
                ShowTree = false;
            }
#endif
        }

        /// <summary>
        /// Handles the Click event of the lnkVerify control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void lnkVerify_Click(object sender, RoutedEventArgs e)
        { // let's check to see if the code we generated compiles or not
            try
            {
                CSharpCodeProvider p = new CSharpCodeProvider();
                CompilerParameters cp = new CompilerParameters();
                string source = "using System;" + Environment.NewLine +
                  "using System.Diagnostics;" + Environment.NewLine +
                  "namespace A { class B { static void Main() {" + Environment.NewLine +
                                tbOut.Text + Environment.NewLine +
                                "}}}";
                CompilerResults cr = p.CompileAssemblyFromSource(cp, source);
                if (cr.Errors.HasErrors)
                {
                    tbOut.Text = source + Environment.NewLine;
                    foreach (CompilerError err in cr.Errors)
                        tbOut.Text += err + Environment.NewLine;
                }
                else
                {
                    MessageBox.Show(
                      "Code has been verified. No errors were found.",
                      "Verification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {

            }
        }
    }
}
