using System;

namespace reportIncident.API.Domain.Models
{
    public class Incident
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
