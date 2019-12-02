using System;
using DroneManager.API.Domain.Models;

namespace DroneManager.API.Domain.Services.Communication
{
    public class SaveDroneResponse : BaseResponse<Drone>
    {
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="drone">Saved drone.</param>
        /// <returns>Response.</returns>
        public SaveDroneResponse(Drone drone) : base(drone)
        { }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SaveDroneResponse(string message) : base(message)
        { }
    }
}
