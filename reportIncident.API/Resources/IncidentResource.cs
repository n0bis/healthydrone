using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reportIncident.API.Resources
{
    public class IncidentResource
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid OperationId { get; set; }
        public Guid DroneId { get; set; }
        public string Details { get; set; }
        public string Damage { get; set; }
        public string Actions { get; set; }
        public string Notes { get; set; }
    }
}
