using Newtonsoft.Json;

namespace alert_state_machine.Models
{
    public class Rain
    {
        [JsonProperty(PropertyName = "3h")]
        public double precipitation { get; set; }
    }
}
