using System;
using System.Collections.Generic;
using System.Text;

namespace alert_state_machine.Models
{
    class Message
    {
        public string alertType { get; set; }
        public string alertLevel { get; set; }
        public Subject subject { get; set; }
        public Subject relatedSubject { get; set; }
    }
}
