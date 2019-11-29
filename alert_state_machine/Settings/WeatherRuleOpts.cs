using System;
namespace alert_state_machine.Settings
{
    public class WeatherRuleOpts
    {
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public double RainPrecipitation { get; set; }
        public double WindSpeed { get; set; }
    }
}
