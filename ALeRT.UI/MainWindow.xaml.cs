using ALeRT.PluginFramework;
using Newtonsoft.Json;
using System;
using System.Collections;
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

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory()));
            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                MessageBox.Show(compositionException.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void queryButton_Click(object sender, RoutedEventArgs e)
        {
            pluginStatusTB.Text = "TYPE PLUGIN STATUS: ";
            resultsTB.Text = "";

            QueryPlugins(queryTB.Text, DetermineTypes(queryTB.Text), (bool)sensitiveCB.IsChecked);
        }

        [ImportMany]
        public IEnumerable<ITypePlugin> TPlugins { get; set; }

        [ImportMany]
        public IEnumerable<IQueryPlugin> QPlugins { get; set; }

        /// <summary>
        /// Method to process all Type plugins.
        /// </summary>
        private List<string> DetermineTypes(string val)
        {
            List<string> typeResultAL = new List<string>();

            foreach (var tPlugins in this.TPlugins)
            {
                if (!tPlugins.Result(val))
                {
                    pluginStatusTB.Inlines.Add(new Run(tPlugins.Name + " ") { Foreground = Brushes.Red });
                }
                else
                {
                    typeResultAL.Add(tPlugins.Name);
                    pluginStatusTB.Inlines.Add(new Bold(new Run(tPlugins.Name + " ") { Foreground = Brushes.Green }));
                }
            }

            return typeResultAL;
        }

        /// <summary>
        /// Method to process all Query plugins.
        /// </summary>
        private void QueryPlugins(string query, List<string> types, bool sensitive)
        {
            foreach (string tType in types) //Cycle through a List<string>
            {
                foreach (var qPlugins in this.QPlugins) //Cycle through all query plugins
                {
                    foreach (string qType in qPlugins.TypesAccepted) //Cycle though a List<string> within the IQueryPlugin interface AcceptedTypes
                    {
                        if (qType == tType) //Match the two List<strings>, one is the AcceptedTypes and the other is the one returned from ITypeQuery
                        {
                            resultsTB.Text += "Type: " + qType.ToString() + " in ";
                            resultsTB.Text += "Plugin: " + qPlugins.Name + "\n";
                            resultsTB.Text += qPlugins.Result(query, qType, sensitive);
                            resultsTB.Text += "\n\n";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Open a dialog prompt to select a file to process.
        /// </summary>
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
