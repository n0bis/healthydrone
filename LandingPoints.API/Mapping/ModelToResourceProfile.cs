using AutoMapper;
using LandingPoints.API.Domain.LandingPointsAPI.Domain.Models;
using LandingPoints.API.Extensions;
using LandingPoints.API.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPoints.API.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<LandingPoint, LandingPointResource>().ForMember(src => src.type, opt => opt.MapFrom(src => src.type.ToDescriptionString()));
        }
    }
}
