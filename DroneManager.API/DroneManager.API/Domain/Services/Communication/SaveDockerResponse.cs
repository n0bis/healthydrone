using System;
using DroneManager.API.Domain.Models;

namespace DroneManager.API.Domain.Services.Communication
{
    public class SaveDockerResponse : BaseResponse
    {
        public DockerContainer DockerContainer { get; private set; }

        public SaveDockerResponse(bool success, string message, DockerContainer dockerContainer) : base(success, message)
        {
            DockerContainer = dockerContainer;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="dockerContainer">Saved dockerContainer.</param>
        /// <returns>Response.</returns>
        public SaveDockerResponse(DockerContainer dockerContainer) : this(true, string.Empty, dockerContainer)
        { }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SaveDockerResponse(string message) : this(false, message, null)
        { }
    }
}
