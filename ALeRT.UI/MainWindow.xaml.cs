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
using GenericParsing;
using System.Windows.Markup;
using System.Data;
using System.Xml;
using System.Xml.Xsl;
using OfficeOpenXml;
using Microsoft.Win32;
using OfficeOpenXml.Style;

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
            string line;

            //resultDS.Reset(); //Looking for a way to clear our the contents from last time without breaking SelectionChanged

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
                    if (file != null) { file.Close(); }
                }
            }
            else
            {
                QueryPlugins(queryTB.Text, DetermineTypes(queryTB.Text), (bool)sensitiveCB.IsChecked);
            }


        }

        DataSet resultDS = new DataSet("Results");

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
                    foreach (string qType in qPlugins.TypesAccepted)  //Cycle though a List<string> within the IQueryPlugin interface AcceptedTypes
                    {
                        if (qType == tType) //Match the two List<strings>, one is the AcceptedTypes and the other is the one returned from ITypeQuery
                        {
                            using (GenericParserAdapter parser = new GenericParserAdapter())
                            {
                                using (TextReader sr = new StringReader(qPlugins.Result(query, qType, sensitive)))
                                {
                                    Random rNum = new Random();

                                    parser.SetDataSource(sr);
                                    parser.ColumnDelimiter = Convert.ToChar(",");
                                    parser.FirstRowHasHeader = true;
                                    parser.MaxBufferSize = 4096;
                                    parser.MaxRows = 500;
                                    parser.TextQualifier = '\"';

                                    DataTable tempTable = parser.GetDataTable();
                                    tempTable.TableName = qPlugins.Name.ToString();
                                    if (!tempTable.Columns.Contains("Query"))
                                    {
                                        DataColumn tColumn = new DataColumn("Query");
                                        tempTable.Columns.Add(tColumn);
                                        tColumn.SetOrdinal(0);
                                    }

                                    foreach (DataRow dr in tempTable.Rows)
                                    {
                                        dr["Query"] = query;
                                    }

                                    if (!resultDS.Tables.Contains(qPlugins.Name.ToString()))
                                    {
                                        resultDS.Tables.Add(tempTable);
                                    }
                                    else
                                    {
                                        resultDS.Tables[qPlugins.Name.ToString()].Merge(tempTable);
                                    }
                                    pluginsLB.DataContext = resultDS.Tables.Cast<DataTable>().Select(t => t.TableName).ToList();
                                }
                            }
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

        private void pluginsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resultsDG.ItemsSource = resultDS.Tables[pluginsLB.SelectedValue.ToString()].DefaultView;
        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //saveFileDialog1.DefaultExt = ".xlsx";
            saveFileDialog1.Filter = "Excel Workbook|*.xlsx";
            //saveFileDialog1.Filter = "CSV|*.csv|Excel Workbook|*.xlsx";
            string format = "yyyyMMMd-HHmm";
            saveFileDialog1.FileName = "Results-" + DateTime.Now.ToString(format);
            saveFileDialog1.Title = "Export As";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

                using (ExcelPackage pck = new ExcelPackage())
                {
                    foreach (DataTable tbl in resultDS.Tables)
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add(tbl.TableName);
                        ws.Cells["A1"].LoadFromDataTable(tbl, true);
                        ws.Cells[ws.Dimension.Address].AutoFilter=true;
                        ws.Cells[ws.Dimension.Address].AutoFitColumns();
                        ws.View.FreezePanes(2, 1);

                        using (var rng = ws.Cells["A1:Z1"])
                        {
                            rng.Style.Font.Bold = true;
                            rng.Style.WrapText = false;
                            rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }
                    }
                    pck.SaveAs(fs);
                }
                fs.Close();
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            resultDS.Clear();
        }
    }
}