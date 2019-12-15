using System;
using System.Collections.Generic;

namespace alert_state_machine.Models
{
    public class EventMessage
    {
        public string name { get; set; }
        public List<Message> data { get; set; }
    }
}
