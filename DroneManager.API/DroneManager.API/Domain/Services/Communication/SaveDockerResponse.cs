using System;
using DroneManager.API.Domain.Models;

namespace DroneManager.API.Domain.Services.Communication
{
    public class SaveDockerResponse : BaseResponse<DockerContainer>
    {
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="dockerContainer">Saved dockerContainer.</param>
        /// <returns>Response.</returns>
        public SaveDockerResponse(DockerContainer dockerContainer) : base(dockerContainer)
        { }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SaveDockerResponse(string message) : base(message)
        { }
    }
}
