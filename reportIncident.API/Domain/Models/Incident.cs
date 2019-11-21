using System;

namespace reportIncident.API.Domain.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Operation { get; set; }
        public string Drone { get; set; }
        public string Details { get; set; }
        public string Damage { get; set; }
        public string Actions { get; set; }
        public string Notes { get; set; }
        //public FileStyleUriParser File { get; set; }
        

    }
}
