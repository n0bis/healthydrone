using System;

namespace alert_state_machine.Models
{
    public class State
    {
        private DateTime TimeStamp { get; set; }
        public bool Triggered { get; set; }
        public bool Handled { get; set; }

        public State()
        {
            this.TimeStamp = DateTime.Now;
            this.Triggered = false;
            this.Handled = false;
        }
    }
}
