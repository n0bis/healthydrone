using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace reportIncident.API.Resources
{
    public class SaveIncidentResource
    {

       
        public DateTime Date { get; private set; } = DateTime.UtcNow;
        public Guid OperationId { get; set; }
        public Guid DroneId { get; set; }
        public string Details { get; set; }
        public string Damage { get; set; }
        public string Actions { get; set; }
        public string Notes { get; set; }
    }
}
