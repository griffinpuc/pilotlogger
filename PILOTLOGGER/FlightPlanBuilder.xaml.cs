using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PILOTLOGGER
{
    /// <summary>
    /// Interaction logic for FlightPlanBuilder.xaml
    /// </summary>
    public partial class FlightPlanBuilder : Window
    {

        int markerCount;
        FlightPlan newPlan;
        public SeriesCollection chartSeries;
        LineSeries altitudeSeries;
        string workingDirectory;

        public FlightPlanBuilder()
        {
            InitializeComponent();
            initializeFlightPlan();
        }

        /* When X is pressed */
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        /* Initialize components */
        private void initializeFlightPlan()
        {

            workingDirectory = Directory.GetCurrentDirectory();

            markerCount = 0;

            //Set map properties
            MainMap.Mode = new RoadMode();
            MainMap.Focus();
            MainMap.Center = new Location(34.0522, -118.2437);
            MainMap.ZoomLevel = 10;

            //Double click events
            MainMap.MouseDoubleClick += new MouseButtonEventHandler(MapWithPushpins_MouseDoubleClick);
            markerListBox.MouseDoubleClick += new MouseButtonEventHandler(goToMarker);

            //Init new flight plan
            newPlan = new FlightPlan();
            newPlan.locationMarkers = new List<LocationMarker>();

            //Init altitude graph
            chartSeries = new SeriesCollection();
            altChart.ChartLegend.Visibility = Visibility.Visible;
            altitudeSeries = new LineSeries();
            altitudeSeries.Values = new ChartValues<double>() { 0 };
            chartSeries.Add(altitudeSeries);
        }

        /* Center on a marker */
        private void goToMarker(object sender, MouseEventArgs e)
        {
            string content = (markerListBox.SelectedItem as ListBoxItem).DataContext.ToString();
            double lat = double.Parse(content.Split(',')[0]);
            double lng = double.Parse(content.Split(',')[1]);

            MainMap.Center = new Location(lat, lng);
            MainMap.ZoomLevel = 12;
        }

        /* Add new marker event */
        private async void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                e.Handled = true;

                Point mousePosition = e.GetPosition(MainMap);
                Location pinLocation = MainMap.ViewportPointToLocation(mousePosition);

                LocationMarker newMarker = new LocationMarker();
                newMarker.markerID = markerCount;

                newMarker.location = new Location(pinLocation.Latitude, pinLocation.Longitude);
                newMarker.latitude = pinLocation.Latitude;
                newMarker.longitude = pinLocation.Longitude;

                await getAltitude(pinLocation.Latitude, pinLocation.Longitude, newMarker);

                Pushpin pin = new Pushpin();
                pin.Location = pinLocation;
                MainMap.Children.Add(pin);

                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = "Marker #" + markerCount;
                newItem.DataContext = newMarker.latitude + "," + newMarker.longitude;
                markerListBox.Items.Add(newItem);

                if (newPlan.locationMarkers.Count > 1)
                {
                    MapPolyline polygon = new MapPolyline();
                    polygon.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
                    polygon.StrokeThickness = 3;
                    polygon.Opacity = 0.7;
                    polygon.Locations = new LocationCollection() { newMarker.location, newPlan.locationMarkers[markerCount - 1].location };

                    MainMap.Children.Add(polygon);
                }

                markerCount++;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        /* Get altitude for a given marker */
        private async Task getAltitude(double lat, double lng, LocationMarker marker)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.airmap.com/elevation/v1/ele/?points=" + lat + "," + lng))
                {
                    var response = await httpClient.SendAsync(request);
                    var s = response.Content.ReadAsStringAsync().Result;
                    JObject json = JObject.Parse(s);

                    string altitudeNum = new string(json["data"].ToString().Where(c => char.IsDigit(c)).ToArray());
                    double altitudeFinal = double.Parse(altitudeNum);

                    marker.altitude = altitudeFinal;
                    newPlan.locationMarkers.Add(marker);

                    altitudeSeries.Values.Add(altitudeFinal);
                    altChart.Series = chartSeries;
                }
            }
            

        }

        /* Save map file */
        private void saveMapFile(object sender, RoutedEventArgs e)
        {
            string file = "";
            foreach (LocationMarker marker in this.newPlan.locationMarkers)
            {
                file += marker.markerID+","+marker.latitude+","+marker.longitude+","+marker.altitude+"!\n";
            }

            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\pilotrc\\flightplans\\" +fileName.Text + ".flight", file);
        }
    }
}
