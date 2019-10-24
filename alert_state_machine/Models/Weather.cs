using System;
using System.Collections.Generic;

namespace alert_state_machine.Models
{
    public class WeatherResponse
    {
        public string name { get; set; }
        public Weather main { get; set; }
        public Wind wind { get; set; }
        public List<Description> weather { get; set; }
    }

    public class Description
    {
        public string main { get; set; }
        public string description { get; set; }
    }

    public class Weather
    {
        public double temp { get; set; }
        public double pressure { get; set; }
        public double humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double sea_level { get; set; }
        public double grnd_level { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public double deg { get; set; }
    }
}
