

using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Entities
{
    public class PuntoData
    {
        public Step[] steps { get; set; }
        public Instruction[] instructions { get; set; }
    }

    public class Step
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public End_Location end_location { get; set; }
        public Polyline polyline { get; set; }
        public Start_Location start_location { get; set; }
        public string travel_mode { get; set; }
        public string encoded_lat_lngs { get; set; }
        public Path[] path { get; set; }
        public Lat_Lngs[] lat_lngs { get; set; }
        public string instructions { get; set; }
        public string maneuver { get; set; }
        public Start_Point start_point { get; set; }
        public End_Point end_point { get; set; }
        public int step_number { get; set; }
    }

    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class End_Location
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Polyline
    {
        public string points { get; set; }
    }

    public class Start_Location
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Start_Point
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class End_Point
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Path
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Lat_Lngs
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Instruction
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public int type { get; set; }
        public string color { get; set; }
    }
}
