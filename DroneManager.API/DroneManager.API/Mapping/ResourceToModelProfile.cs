using System;
using AutoMapper;
using DroneManager.API.Domain.Models;
using DroneManager.API.Resources;

namespace DroneManager.API.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SaveDroneResource, Drone>();
        }
    }
}
