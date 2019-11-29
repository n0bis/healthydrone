using System;
namespace alert_state_machine.Models
{
    public class WeatherRuleResponse : BaseResponse
    {
        public WeatherRuleResponse(bool success, string message) : base(success, message)
        {
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <returns>Response.</returns>
        public WeatherRuleResponse() : this(true, string.Empty)
        { }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public WeatherRuleResponse(string message) : this(false, message)
        { }
    }
}
