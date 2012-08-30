using ALeRT.PluginFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ALeRT.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void queryButton_Click(object sender, RoutedEventArgs e)
        {
            DetermineTypes(queryTB.Text);
        }

        [ImportMany]
        public IEnumerable<ITypePlugin> TPlugins { get; set; }

        /// <summary>
        /// Method to load all Type Plugins.
        /// </summary>
        private void DetermineTypes(string val)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory()));
            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            pluginStatusTB.Text = "Type Plugins Status: ";
            resultsTB.Text = "";

            foreach (var tPlugins in this.TPlugins)
            {
                if (!tPlugins.Result(val))
                {
                    pluginStatusTB.Inlines.Add(new Run(tPlugins.Name + " ") { Foreground = Brushes.Red });
                }
                else
                {
                    pluginStatusTB.Inlines.Add(new Run(tPlugins.Name + " ") { Foreground = Brushes.Green });
                }
                //resultsTB.Text += "Category: " + tPlugins.PluginCategory + "\nName: " + tPlugins.Name + "\nThe result is: " + tPlugins.Result(val) + "\n\n";
            }

            //foreach (var tPlugins in this.TPlugins)
            //{
            //    resultsTB.Text += "Category: " + tPlugins.PluginCategory + "\nName: " + tPlugins.Name + "\nThe result is: " + tPlugins.Result(val) + "\n\n";
            //}
        }

        private void QueryPlugins()
        {
            throw new NotImplementedException();
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "All Files|*.*";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                queryTB.Text = dlg.FileName;
            }
        }
    }
}
