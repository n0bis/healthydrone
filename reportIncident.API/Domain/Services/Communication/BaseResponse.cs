using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reportIncident.API.Domain.Services.Communication;

namespace reportIncident.API.Domain.Services.Communication
{
    public abstract class BaseResponse
    {
        public bool Succes { get; protected set; }
        public string Message { get; protected set; }

        public BaseResponse(bool succes, string message)
        {
            Succes = succes;
            Message = message;
        }
    }
}
