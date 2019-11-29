using Newtonsoft.Json;

namespace alert_state_machine.Models
{
    public class Rain
    {
        [JsonProperty(PropertyName = "1h")]
        public double precipitation { get; set; }

    }
}
