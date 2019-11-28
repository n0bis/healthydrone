using System;
using System.ComponentModel.DataAnnotations;
using DroneManager.API.Domain.Models;

namespace DroneManager.API.Resources
{
    public class SaveDroneResource
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string OperationId { get; set; }
        [Required]
        public double velocity { get; set; }
        [Required]
        public Location CurrentLocation { get; set; }
        [Required]
        public Location HomeLocation { get; set; }
    }
}
