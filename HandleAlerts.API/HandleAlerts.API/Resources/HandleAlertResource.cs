using System;
using System.ComponentModel.DataAnnotations;

namespace HandleAlerts.API.Resources
{
    public class HandleAlertResource
    {
        [Required]
        public Guid UasOperation { get; set; }
        [Required]
        public Guid DroneID { get; set; }
        [Required]
        public string AlertType { get; set; }
    }
}
