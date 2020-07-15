using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PILOTLOGGER
{
    /// <summary>
    /// Interaction logic for LogBox.xaml
    /// </summary>
    public partial class LogBox : Window
    {

        public SeriesCollection sc;
        Dictionary<LineSeries, bool> seriesVisiblity = new Dictionary<LineSeries, bool>();

        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);


        const uint MF_BYCOMMAND = 0x00000000;
        const uint MF_GRAYED = 0x00000001;

        const uint SC_CLOSE = 0xF060;

        public LogBox()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Disable close button
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            if (hMenu != IntPtr.Zero)
            {
                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
            }
        }

        public void setSeries(string series)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                sc = new SeriesCollection();
                chart.LegendLocation = LegendLocation.Right;
                chart.ChartLegend.Visibility = Visibility.Visible;

                foreach (string name in series.Split(','))
                {
                    LineSeries s = new LineSeries();
                    s.Name = name;
                    s.Values = new ChartValues<double>();
                    s.Title = name;

                    sc.Add(s);
                    seriesVisiblity.Add(s, true);

                }

                chart.Series = sc;
            }));
        }

        public void setSeriesLine(string[] line)
        {
            if(line.Length == sc.Count)
            {
                int i = 0;
                foreach (LineSeries series in sc)
                {
                    series.Values.Add(double.Parse(line[i]));
                    i++;

                    if (series.Values.Count > 15)
                    {
                        series.Values.RemoveAt(0);
                    }
                }

                Dispatcher.Invoke(new Action(() =>
                {
                    chart.Series = sc;
                }));
            }

        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
