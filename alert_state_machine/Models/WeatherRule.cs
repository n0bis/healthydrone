using System;
namespace alert_state_machine.Models
{
    public class WeatherRule
    {
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public double RainPrecipitation { get; set; }
        public double WindSpeed { get; set; }

        public WeatherRuleResponse ValidateRule(WeatherResponse weather)
        {
            if (weather.main.temp < MinTemp)
            {
                return new WeatherRuleResponse($"temperature below {MinTemp}");
            }

            if (weather.main.temp > MaxTemp)
            {
                return new WeatherRuleResponse($"temperature above {MaxTemp}");
            }

            if (weather?.rain?.precipitation > RainPrecipitation)
            {
                return new WeatherRuleResponse($"precipitation is greather than {RainPrecipitation}mm");
            }

            if (weather?.wind?.speed > WindSpeed)
            {
                return new WeatherRuleResponse($"wind speed greater than {WindSpeed}m/s");
            }

            return new WeatherRuleResponse();
        }
    }
}
