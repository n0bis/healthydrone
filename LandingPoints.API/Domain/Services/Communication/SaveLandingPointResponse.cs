using LandingPoints.API.Domain.LandingPointsAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPoints.API.Domain.Services.Communication
{
    public class SaveLandingPointResponse : BaseResponse
    {
        public LandingPoint LandingPoint { get; private set; }

        private SaveLandingPointResponse(bool success, string message, LandingPoint landingPoint) : base(success, message)
        {
            LandingPoint = landingPoint;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="category">Saved category.</param>
        /// <returns>Response.</returns>
        public SaveLandingPointResponse(LandingPoint landingPoint) : this(true, string.Empty, landingPoint)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SaveLandingPointResponse(string message) : this(false, message, null)
        { }
    }
}
