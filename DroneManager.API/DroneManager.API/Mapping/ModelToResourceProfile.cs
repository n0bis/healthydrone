using System;
using AutoMapper;
using DroneManager.API.Domain.Models;
using DroneManager.API.Resources;

namespace DroneManager.API.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<SaveDroneResource, Drone>();
        }
    }
}
