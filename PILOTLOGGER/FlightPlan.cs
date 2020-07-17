using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PILOTLOGGER
{
    class FlightPlan
    {
        public string planName { get; set; }
        public string planDesc { get; set; }
        public List<LocationMarker> locationMarkers { get; set; }

    }

    class LocationMarker
    {
        public int markerID { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double altitude { get; set; }

        public Location location { get; set; }
    }
}
