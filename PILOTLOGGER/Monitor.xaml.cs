using AdonisUI;
using HelixToolkit.Wpf;
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
using System.Windows.Media;
using System.Windows.Media.Media3D;

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
        public SeriesCollection chartSeries;
        public string baseDirectory;

        Pushpin droneLocationPin;

        private Model3DGroup model;
        ModelVisual3D device3D;


        public Monitor()
        {
            AdonisUI.ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.DarkColorScheme);

            InitializeComponent();
            DataContext = this;

            schemaCodeList = new List<string>();
            serialValues = new LineSeries[schemaCodeList.Count];
            baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PilotRC";
        }


        #region INotifyPropertyChanged Members

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

        private double _Latitude = 0;
        public double Latitude
        {
            get { return _Latitude; }
            set
            {
                _Latitude = value;
                OnPropertyChanged();
            }
        }

        private double _Longitude = 0;
        public double Longitude
        {
            get { return _Longitude; }
            set
            {
                _Longitude = value;
                OnPropertyChanged();
            }
        }

        private double _Roll = 0;
        public double Roll
        {
            get { return _Roll; }
            set
            {
                _Roll = value;
                OnPropertyChanged();
            }
        }

        private double _RollLowerLimit = -10;
        public double RollLowerLimit
        {
            get { return _RollLowerLimit; }
            set
            {
                _RollLowerLimit = value;
                OnPropertyChanged();
            }
        }

        private double _RollUpperLimit = 10;
        public double RollUpperLimit
        {
            get { return _RollUpperLimit; }
            set
            {
                _RollUpperLimit = value;
                OnPropertyChanged();
            }
        }

        private double _Pitch = 0;
        public double Pitch
        {
            get { return _Pitch; }
            set
            {
                _Pitch = value;
                OnPropertyChanged();
            }
        }

        private double _Heading = 0;
        public double Heading
        {
            get { return _Heading; }
            set
            {
                _Heading = value;
                OnPropertyChanged();
            }
        }

        private double _HeadingUpperLimit = 10;
        public double HeadingUpperLimit
        {
            get { return _HeadingUpperLimit; }
            set
            {
                _HeadingUpperLimit = value;
                OnPropertyChanged();
            }
        }

        private double _HeadingLowerLimit = -10;
        public double HeadingLowerLimit
        {
            get { return _HeadingLowerLimit; }
            set
            {
                _HeadingLowerLimit = value;
                OnPropertyChanged();
            }
        }

        private double _PitchLowerLimit = -80;
        public double PitchLowerLimit
        {
            get { return _PitchLowerLimit; }
            set
            {
                _PitchLowerLimit = value;
                OnPropertyChanged();
            }
        }

        private double _PitchUpperLimit = 80;
        public double PitchUpperLimit
        {
            get { return _PitchUpperLimit; }
            set
            {
                _PitchUpperLimit = value;
                OnPropertyChanged();
            }
        }

        private double _Yaw = 0;
        public double Yaw
        {
            get { return _Yaw; }
            set
            {
                _Yaw = value;
                OnPropertyChanged();
            }
        }


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

        #region Live Chart Members

        /* Initialize the graph and set properties */
        public void initChart()
        {
            chartSeries = new SeriesCollection();
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

        /* Set chart to graph the first value by default */
        public void setChartDefault()
        {
            chartSeries.Add(serialValues[0]);
            graphcombo.SelectedItem = graphcombo.Items[0];
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
                    if (chartValues.Count > 20)
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
        }

        #endregion

        #region GPS Mapping Members

        /* Initialize default map properties */
        public void initMap()
        {
            MainMap.Mode = new AerialMode(); //or RoadMode();
            MainMap.Focus();
            MainMap.Center = new Location(34.0522, -118.2437);
            MainMap.ZoomLevel = 10;

            ControlTemplate template = (ControlTemplate)this.FindResource("DroneLocationTemplate");
            droneLocationPin = new Pushpin();
            droneLocationPin.Template = template;
            droneLocationPin.Location = MainMap.Center;
            MainMap.Children.Add(droneLocationPin);

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
            foreach (LocationMarker marker in flightPlan.locationMarkers)
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

                markerCount++;
            }
        }

        #endregion


        /* Init 3D Model */
        public void initModel()
        {
            try
            {
                device3D = new ModelVisual3D();
                ModelImporter import = new ModelImporter();

                //Load the 3D model file
                device3D.Content = import.Load(baseDirectory + "\\models\\vtolcolor.obj");

                // Add to view port
                Viewport.Children.Add(device3D);
            }
            catch (Exception ex)
            {
                MessageBox.Show("VTOL Model Not found \n" + ex.Message);
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

        /* Change visuals */
        public void modifyValues(double velocity, double acceleration, double altitude, double lat, double lng, double roll, double pitch, double yaw, double heading)
        {
            int velConstant = 120; //Top speed is 120 mph
            int accelConstant = 40; //Top acceleration is 40m/s^2
            int altitudeConstant = 500; //Top altitude is 500m

            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this.Altitude = altitude;
            this.Latitude = lat;
            this.Longitude = lng;
            this.Roll = roll;
            this.Pitch = pitch;
            this.Yaw = yaw;
            this.Heading = heading;

            this.VelocityCurve = (int)(((velocity * 240) / velConstant) - 120);
            this.AccelerationCurve = (int)(((acceleration * accelConstant) / 100) - 120);
            this.AltitudeCurve = (int)(((altitude * 240) / altitudeConstant) - 120);
            this.RollLowerLimit = roll - 10;
            this.RollUpperLimit = roll + 10;
            this.PitchLowerLimit = (pitch + 90) - 10;
            this.PitchUpperLimit = (pitch + 90) + 10;
            this.HeadingLowerLimit = (heading) - 10;
            this.HeadingUpperLimit = (heading) + 10;

            /* 3D MODEL ROTATIONS */
            var axis = new Vector3D(0, 0, 1);
            var angle = 5;

            Dispatcher.Invoke(new Action(() =>
            {
                droneLocationPin.Location = new Location(lat, lng);
            }));

            //try
            //{


            //        var matrix = device3D.Transform.Value;
            //        matrix.Rotate(new Quaternion(axis, angle));
            //        device3D.Transform = new MatrixTransform3D(matrix);
            //    }));
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.StackTrace);
            //}
            /* END MODEL ROTATING */


        }

        /* Toggle dark overlay */
        public void toggleOverlay()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (darkGrid.Visibility == System.Windows.Visibility.Visible)
                {
                    darkGrid.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    darkGrid.Visibility = System.Windows.Visibility.Visible;
                }
            }));
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}