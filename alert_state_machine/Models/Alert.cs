using System;
namespace alert_state_machine.Models
{
    public class Alert
    {
        public Guid droneId { get; set; }
        public string type { get; set; }
        public string reason { get; set; }
        public string name { get; set; }
    }
}
