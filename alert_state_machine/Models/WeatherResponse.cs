using System.Collections.Generic;

namespace alert_state_machine.Models
{
    public class WeatherResponse
    {
        public string name { get; set; }
        public Weather main { get; set; }
        public Wind wind { get; set; }
        public List<Description> weather { get; set; }
        public Rain rain { get; set; }
    }
}
