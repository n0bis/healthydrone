using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using reportIncident.API.Domain.Models;
using reportIncident.API.Resources;

namespace reportIncident.API.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SaveIncidentResource, Incident>();
        }
    }
}
