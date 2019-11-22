using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace alert_state_machine.Models
{
    public class DataOuter
    {
        public Dictionary<string, DataMiddle> data { get; set; }
    }
}
