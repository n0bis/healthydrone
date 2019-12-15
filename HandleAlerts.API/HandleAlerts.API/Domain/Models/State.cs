using System;
namespace HandleAlerts.API.Domain.Models
{
    public class State
    {
        public ProcessState CurrentState { get; set; }
        private DateTime TimeStamp { get; set; }
        public bool Triggered { get; set; }
        public bool Handled { get; set; }

        public State()
        {
            this.CurrentState = ProcessState.Inactive;
            this.TimeStamp = DateTime.Now;
            this.Triggered = false;
            this.Handled = false;
        }
    }

    public enum ProcessState
    {
        Inactive,
        Active,
        Raised
    }
}
