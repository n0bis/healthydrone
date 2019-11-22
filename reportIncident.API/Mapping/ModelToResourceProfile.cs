using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reportIncident.API.Domain.Models;
using reportIncident.API.Resources;
using AutoMapper;

namespace reportIncident.API.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Incident, IncidentResource>();
        }
    }
}
