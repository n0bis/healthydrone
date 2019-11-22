using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reportIncident.API.Domain.Models;

namespace reportIncident.API.Domain.Services.Communication
{
    public class SaveIncidentResponse : BaseResponse
    {
        public Incident Incident { get; private set; }

        private SaveIncidentResponse(bool succes, string message, Incident incident) : base(succes, message)
        {
            Incident = incident;
        }
        public SaveIncidentResponse(Incident incident) : this(true, string.Empty, incident)
        { }

        public SaveIncidentResponse(string message) : this(false, message, null)
        { }
    }
}
