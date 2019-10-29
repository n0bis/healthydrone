using System;
namespace common.utm.service.Models
{
    public class Flight
    {
        public Guid uasFlight { get; set; }
        public string operationName { get; set; }
        public Guid uasOperation { get; set; }
        public UAS uas { get; set; }
        public Pilot pilotInfo { get; set; }
        public Coordinates coordinates { get; set; }
    }
}
