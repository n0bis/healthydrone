using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reportIncident.API.Domain.Models;

namespace reportIncident.API.Domain.Services.Communication
{
    public class IncidentResponse : BaseResponse
    {
        public Incident Incident { get; private set; }

        private IncidentResponse(bool succes, string message, Incident incident) : base(succes, message)
        {
            Incident = incident;
        }
        public IncidentResponse(Incident incident) : this(true, string.Empty, incident)
        { }

        public IncidentResponse(string message) : this(false, message, null)
        { }
    }
}
