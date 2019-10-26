using AutoMapper;
using LandingPoints.API.Domain.LandingPointsAPI.Domain.Models;
using LandingPoints.API.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPoints.API.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SaveLandingPointResource, LandingPoint>();
        }
    }
}
