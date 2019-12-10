using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DroneSimulator.API.Domain.Models;
using DroneSimulator.API.Domain.Services;
using DroneSimulator.API.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using utm_service;
using utm_service.Models;

namespace DroneSimulator.API.Services
{
    public class DroneSim : IDroneSim
    {
        private static Drone _drone;
        private readonly UTMService _utmService;

        public DroneSim(IOptions<DroneOpts> droneOpts, IOptions<UTMOpts> utmOpts)
        {
            _drone = new Drone {
                Id = droneOpts.Value.id,
                OperationId = droneOpts.Value.operationid,
                Velocity = droneOpts.Value.velocity,
                Location = droneOpts.Value.location,
                HomeLocation = droneOpts.Value.homelocation
            };
            this._utmService = new UTMService(
                utmOpts.Value.clientid,
                utmOpts.Value.clientsecret,
                utmOpts.Value.username,
                utmOpts.Value.password
            );
        }

        public async Task SendOnMission(List<Location> locations, CancellationToken cancellationToken)
        {
            await TakeOffNotification();

            foreach (var (location, index) in locations.WithIndex())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                await MoveTo(_drone.Location, location, cancellationToken);
            }

            await LandNotification();
        }

        public async Task SendHome(CancellationToken cancellationToken)
        {
            await MoveTo(_drone.Location, _drone.HomeLocation, cancellationToken);
            await LandNotification();
        }

        public async Task LandAtLocation(Location location, CancellationToken cancellationToken)
        {
            await MoveTo(_drone.Location, location, cancellationToken);
            await LandNotification();
        }

        private async Task MoveTo(Location startPoint, Location endPoint, CancellationToken cancellationToken)
        {
            var startLocation = startPoint;
            var endLocation = endPoint;
            var distanceBetweenPoints = LatLonSpherical.CalculateDistanceBetweenLocations(startLocation, endLocation) * 1000; // multiply by 1000 to get in meters
            var timeRequired = distanceBetweenPoints / _drone.Velocity;

            for (int i = 0; i < timeRequired; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                var bearing = LatLonSpherical.CalculateBearing(startLocation, endLocation);
                var distanceInKm = _drone.Velocity / 1000;
                var intermediaryLocation = LatLonSpherical.CalculateDestinationLocation(startLocation, bearing, distanceInKm);

                var coordinates = new Coordinates { latitude = intermediaryLocation.latitude, longitude = intermediaryLocation.longitude };
                var track = new Track { location = coordinates, timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") };
                var response = await this._utmService.Tracking.FlightTrack(_drone.Id,
                    _drone.OperationId, track);
                if (response)
                {
                    _drone.Location = intermediaryLocation;
                    Console.WriteLine($"operation: {_drone.OperationId} drone: {_drone.Id} latitude: {track.location.latitude} longitude: {track.location.longitude}");
                }
                else
                {
                    Console.WriteLine("Error");
                }

                startLocation = intermediaryLocation;
            }
        }

        public async Task Hover(CancellationToken cancellationToken)
        {
            while(true)
            {
                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();

                var coordinates = new Coordinates { latitude = _drone.Location.latitude, longitude = _drone.Location.longitude };
                var track = new Track { location = coordinates, timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") };
                var response = await this._utmService.Tracking.FlightTrack(_drone.Id,
                    _drone.OperationId, track);
                if (response)
                {
                    _drone.Location = new Location { latitude = coordinates.latitude, longitude = coordinates.longitude };
                    Console.WriteLine($"hover - operation: {_drone.OperationId} drone: {_drone.Id} latitude: {track.location.latitude} longitude: {track.location.longitude}");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        private async Task TakeOffNotification()
        {
            await this._utmService.Tracking.TakeOff(_drone.Id, _drone.OperationId);
        }

        private async Task LandNotification()
        {
            await this._utmService.Tracking.Land(_drone.Id, _drone.OperationId);
        }
    }
}
