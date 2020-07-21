using AdonisUI;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace PILOTLOGGER
{

    public partial class Monitor : Window, INotifyPropertyChanged
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
        private LineSeries altitudeSeries;
        public SeriesCollection altchartSeries;
        public SeriesCollection chartSeries;
        public string baseDirectory;


        public Monitor()
        {
            AdonisUI.ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.DarkColorScheme);

            InitializeComponent();
            DataContext = this;

            schemaCodeList = new List<string>();
            serialValues = new LineSeries[schemaCodeList.Count];
            baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PilotRC";

            initMap();
            initAltChart();

            FlightPlan plan = parseFlightplan("mountains.flight");
            drawFlightPlan(plan);

        }

        /* Live binded variables */
        private double _Velocity = 0;
        public double Velocity
        {
            get { return _Velocity; }
            set
            {
                _Velocity = value;
                OnPropertyChanged();
            }
        }

        private double _Acceleration = 0;
        public double Acceleration
        {
            get { return _Acceleration; }
            set
            {
                _Acceleration = value;
                OnPropertyChanged();
            }
        }

        private double _Altitude = 0;
        public double Altitude
        {
            get { return _Altitude; }
            set
            {
                _Altitude = value;
                OnPropertyChanged();
            }
        }

        private int _AltitudeCurve = -120;
        public int AltitudeCurve
        {
            get { return _AltitudeCurve; }
            set
            {
                _AltitudeCurve = value;
                OnPropertyChanged();
            }
        }

        private int _VelocityCurve = -120;
        public int VelocityCurve
        {
            get { return _VelocityCurve; }
            set
            {
                _VelocityCurve = value;
                OnPropertyChanged();
            }
        }

        private double _AccelerationCurve = -120;
        public double AccelerationCurve
        {
            get { return _AccelerationCurve; }
            set
            {
                _AccelerationCurve = value;
                OnPropertyChanged();
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        #endregion

        /* Initialize default map properties */
        private void initMap()
        {
            MainMap.Mode = new RoadMode();
            MainMap.Focus();
            MainMap.Center = new Location(34.0522, -118.2437);
            MainMap.ZoomLevel = 10;
        }

        /* Initialize the altitude graph */
        private void initAltChart()
        {
            altchartSeries = new SeriesCollection();
            altchart.ChartLegend.Visibility = Visibility.Visible;
            altitudeSeries = new LineSeries();
            altitudeSeries.Values = new ChartValues<double>() { 0 };
            altchartSeries.Add(altitudeSeries);
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
                //testlabel.Content = labelText;
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

        /* Parse flightplan file into object */
        private FlightPlan parseFlightplan(string fileName)
        {
            string schemaCode = File.ReadAllText(baseDirectory + "\\flightplans\\" + fileName);
            string parsedCode = schemaCode.Remove(schemaCode.Length - 2).Replace("\n", "");

            FlightPlan loadedPlan = new FlightPlan();
            loadedPlan.locationMarkers = new List<LocationMarker>();

            foreach (string line in parsedCode.Split('!'))
            {
                string[] dataPoints = line.Split(',');
                LocationMarker newMarker = new LocationMarker();
                newMarker.markerID = Convert.ToInt32(dataPoints[0]);
                newMarker.latitude = double.Parse(dataPoints[1]);
                newMarker.longitude = double.Parse(dataPoints[2]);
                newMarker.altitude = double.Parse(dataPoints[3]);
                newMarker.location = new Location(newMarker.latitude, newMarker.longitude);

                loadedPlan.locationMarkers.Add(newMarker);
            }

            return loadedPlan;

        }

        /* Draw flightplan on map */
        private void drawFlightPlan(FlightPlan flightPlan)
        {
            int markerCount = 0;
            foreach(LocationMarker marker in flightPlan.locationMarkers)
            {
                Pushpin pin = new Pushpin();
                pin.Location = marker.location;
                MainMap.Children.Add(pin);

                if (markerCount > 1)
                {
                    MapPolyline polygon = new MapPolyline();
                    polygon.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
                    polygon.StrokeThickness = 3;
                    polygon.Opacity = 0.7;
                    polygon.Locations = new LocationCollection() { marker.location, flightPlan.locationMarkers[markerCount - 1].location };

                    MainMap.Children.Add(polygon);
                }

                altitudeSeries.Values.Add(marker.altitude);
                altchart.Series = altchartSeries;

                markerCount++;
            }
        }

        /* Change visuals */
        public void modifyValues(double velocity, double acceleration, double altitude)
        {
            int velConstant = 120; //Top speed is 120 mph
            int accelConstant = 40; //Top acceleration is 40m/s^2
            int altitudeConstant = 500; //Top altitude is 500m

            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this.Altitude = altitude;

            this.VelocityCurve = (int)(((velocity * 240)/ velConstant) -120);
            this.AccelerationCurve = (int)(((acceleration * accelConstant) / 100) - 120);
            this.AltitudeCurve = (int)(((altitude * 240) / altitudeConstant) - 120);
        }

    }
}
