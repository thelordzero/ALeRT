using ALeRT.PluginFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
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
            //pluginStatusTB.Text = "TYPE PLUGINS LOADED: ";
            resultsTB.Text = "";

            string line;
            ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;
            if (File.Exists(queryTB.Text) && (bool)listCB.IsChecked)
            {
                StreamReader file = null;
                try
                {
                    file = new StreamReader(queryTB.Text);
                    while ((line = file.ReadLine()) != null)
                    {
                        QueryPlugins(line, DetermineTypes(line), (bool)sensitiveCB.IsChecked);
                    }
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }
            else
            {
                QueryPlugins(queryTB.Text, DetermineTypes(queryTB.Text), (bool)sensitiveCB.IsChecked);
            }
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
                if (tPlugins.Result(val))
                {
                    typeResultAL.Add(tPlugins.Name);
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
                            //resultsTB.Text += "Type: " + qType.ToString() + " in ";
                            //resultsTB.Text += "Plugin: " + qPlugins.Name + " (" + query + ")\n";
                            resultsTB.Text += "\"" + qType.ToString() + qPlugins.Name + "(" + query + ")\": " + qPlugins.Result(query, qType, sensitive) + "\n\n";
                            resultsTB.Text += "\n\n";

                            //XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"" + qType.ToString() + qPlugins.Name + "" + query + "\": "  + qPlugins.Result(query, qType, sensitive) + "}");
                            //StringBuilder sb = new StringBuilder();
                            //System.IO.TextWriter tr = new System.IO.StringWriter(sb);
                            //XmlTextWriter wr = new XmlTextWriter(tr);
                            //wr.Formatting = System.Xml.Formatting.Indented;
                            //doc.Save(wr);
                            //wr.Close();
                            //resultsTB.Text = sb.ToString();

                            //dynamic newValue = JsonConvert.DeserializeObject<DynamicDictionary>("{\"" + qType.ToString() + qPlugins.Name + "" + query + "\": " + qPlugins.Result(query, qType, sensitive) + "}");
                            //string role = newValue.Mail[0];
                            //resultsTB.Text = "";
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

        private void csvButton_Click(object sender, RoutedEventArgs e)
        {
            dynamic stuff = JsonConvert.ToString(resultsTB.Text);
            resultsTB.Text = stuff.ToString();
        }
    }
}