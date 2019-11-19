using System;
using DroneManager.API.Domain.Models;

namespace DroneManager.API.Domain.Services.Communication
{
    public class SaveDroneResponse : BaseResponse
    {
        public Drone Drone { get; private set; }

        public SaveDroneResponse(bool success, string message, Drone drone) : base(success, message)
        {
            Drone = drone;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="drone">Saved drone.</param>
        /// <returns>Response.</returns>
        public SaveDroneResponse(Drone drone) : this(true, string.Empty, drone)
        { }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SaveDroneResponse(string message) : this(false, message, null)
        { }
    }
}
