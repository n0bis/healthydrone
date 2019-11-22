using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace reportIncident.API.Resources
{
    public class SaveIncidentResource
    {
        [Required]
        [MaxLength(30)]
        public string Id { get; set; }
    }
}
