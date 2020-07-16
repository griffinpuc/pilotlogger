using AdonisUI;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace PILOTLOGGER
{

    public partial class Monitor : Window
    {
        // Disable close button
        //Variables for disallowing x in corner
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
        const uint MF_BYCOMMAND = 0x00000000;
        const uint MF_GRAYED = 0x00000001;
        const uint SC_CLOSE = 0xF060;
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            if (hMenu != IntPtr.Zero)
            {
                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
            }
        }
        //End of that block


        private List<string> schemaCodeList;
        private LineSeries[] serialValues;
        public SeriesCollection chartSeries;

        public Monitor()
        {
            AdonisUI.ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.DarkColorScheme);

            InitializeComponent();

            schemaCodeList = new List<string>();
            serialValues = new LineSeries[schemaCodeList.Count];

        }

        /* Initialize the graph and set properties */
        public void initChart()
        {
            chartSeries = new SeriesCollection();
            chart.LegendLocation = LegendLocation.Right;
            chart.ChartLegend.Visibility = Visibility.Visible;
            chart.DisableAnimations = true;
        }

        /* Initialize line series from schema values */
        public void setValues(string schemaCode)
        {
            int index = 0;
            string[] schemaCodes = schemaCode.Split(',');
            serialValues = new LineSeries[schemaCodes.Length];

            foreach (string code in schemaCodes)
            {
                schemaCodeList.Add(code);

                //New line series for every value
                LineSeries newSeries = new LineSeries();
                newSeries.Name = code;
                newSeries.Values = new ChartValues<double>() { 0 };
                newSeries.Title = code;

                serialValues[index] = newSeries;

                //Add to combobox dropdown for user selection
                Dispatcher.Invoke(new Action(() =>
                {
                    MenuItem item = new MenuItem { Header = code };
                    item.Click += setGraphValue;
                    graphcombo.Items.Add(item);
                }));

                index++;
            }
        }

        /* Modify contents of test label - may be removed */
        public void modifyContents(string labelText)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                testlabel.Content = labelText;
            }));
        }

        /* Set chart to graph the first value by default */
        public void setChartDefault()
        {
            chartSeries.Add(serialValues[0]);
        }

        /* Modify which value is graphed from combobox */
        public void setGraphValue(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                graphcombo.SelectedItem = sender;
                graphcombo.IsDropDownOpen = false;
                chartSeries.Clear();
                chartSeries.Add(serialValues[graphcombo.SelectedIndex]);
            }));
        }

        /* Send serial port data string and add to graph line series */
        public void addValues(string values)
        {

            //Check to make sure data lines up, discards broken lines
            if (values.Split(',').Length == serialValues.Length)
            {
                int index = 0;
                foreach (string value in values.Split(','))
                {
                    IChartValues chartValues = serialValues[index].Values;
                    chartValues.Add(double.Parse(value));

                    //Modifys chart to only graph x values at a time (simulate real time graphing)
                    if (chartValues.Count > 100)
                    {
                        chartValues.RemoveAt(0);
                    }

                    index++;
                }

                //Update chart every cycle to reflect new changes
                Dispatcher.Invoke(new Action(() =>
                {
                    chart.Series = chartSeries;
                }));
            }

            Console.WriteLine();
        }

    }
}
