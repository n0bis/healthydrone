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
        private readonly double _earthRadius = 6371; // radius in km
        private readonly Drone _drone;
        private readonly UTMService _utmService;

        public DroneSim(IOptions<DroneOpts> droneOpts, IOptions<UTMOpts> utmOpts)
        {
            this._drone = new Drone {
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
            var startLocation = this._drone.Location;
            var endLocation = locations.LastOrDefault();
            var distanceBetweenPoints = CalculateDistanceBetweenLocations(startLocation, endLocation) * 1000; // multiply by 1000 to get in meters
            var timeRequired = distanceBetweenPoints / this._drone.Velocity;
            this._drone.mission = new Mission { TimeEstimated = TimeSpan.FromSeconds(timeRequired), Waypoints = locations };

            foreach (var (location, index) in locations.WithIndex())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Eh stop");
                    cancellationToken.ThrowIfCancellationRequested();
                }
                    
                var nextLocation = locations.ElementAtOrDefault(index + 1) ?? endLocation;
                await MoveTo((index == 0) ? startLocation : location, nextLocation, cancellationToken);
            }
        }

        public async Task SendHome(CancellationToken cancellationToken)
        {
            await MoveTo(this._drone.Location, this._drone.HomeLocation, cancellationToken);
            await LandNotification();
        }

        public async Task LandAtLocation(Location location, CancellationToken cancellationToken)
        {
            await MoveTo(this._drone.Location, location, cancellationToken);
            await LandNotification();
        }

        private async Task MoveTo(Location startPoint, Location endPoint, CancellationToken cancellationToken)
        {
            var startLocation = startPoint;
            var endLocation = endPoint;
            var distanceBetweenPoints = CalculateDistanceBetweenLocations(startLocation, endLocation) * 1000; // multiply by 1000 to get in meters
            var timeRequired = distanceBetweenPoints / this._drone.Velocity;

            for (int i = 0; i < timeRequired; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("HALT");
                    cancellationToken.ThrowIfCancellationRequested();
                }

                var bearing = CalculateBearing(startLocation, endLocation);
                var distanceInKm = this._drone.Velocity / 1000;
                var intermediaryLocation = CalculateDestinationLocation(startLocation, bearing, distanceInKm);

                var coordinates = new Coordinates { latitude = intermediaryLocation.latitude, longitude = intermediaryLocation.longitude };
                var track = new Track { location = coordinates, timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") };
                Thread.Sleep(1000);
                /*var response = await this._utmService.Tracking.FlightTrack(this._drone.Id,
                    this._drone.OperationId, track);
                if (response)
                {
                    this._drone.Location = intermediaryLocation;
                    Console.WriteLine($"operation: {this._drone.OperationId} drone: {this._drone.Id} latitude: {track.location.latitude} longitude: {track.location.longitude}");
                }
                else
                {
                    Console.WriteLine("Error");
                }*/

                startLocation = intermediaryLocation;
            }
        }

        public async Task Hover(CancellationToken cancellationToken)
        {
            while(true)
            {
                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();

                var coordinates = new Coordinates { latitude = this._drone.Location.latitude, longitude = this._drone.Location.longitude };
                var track = new Track { location = coordinates, timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") };
                /*var response = await this._utmService.Tracking.FlightTrack(this._drone.Id,
                    this._drone.OperationId, track);
                if (response)
                {
                    Console.WriteLine($"operation: {this._drone.OperationId} drone: {this._drone.Id} latitude: {track.location.latitude} longitude: {track.location.longitude}");
                }
                else
                {
                    Console.WriteLine("Error");
                }*/
            }
        }

        private async Task TakeOffNotification()
        {
            await this._utmService.Tracking.TakeOff(this._drone.Id, this._drone.OperationId);
        }

        private async Task LandNotification()
        {
            await this._utmService.Tracking.Land(this._drone.Id, this._drone.OperationId);
        }

        private double DegreeToRadian(double degree)
        {
            return (degree * Math.PI / 180);
        }

        private double RadianToDegree(double radian)
        {
            return (radian * 180 / Math.PI);
        }

        // Calculate the (initial) bearing between two points, in degrees
        private double CalculateBearing(Location startPoint, Location endPoint)
        {
            double lat1 = DegreeToRadian(startPoint.latitude);
            double lat2 = DegreeToRadian(endPoint.latitude);
            double deltaLon = DegreeToRadian(endPoint.longitude - startPoint.longitude);

            double y = Math.Sin(deltaLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(deltaLon);
            double bearing = Math.Atan2(y, x);

            // since atan2 returns a value between -180 and +180, we need to convert it to 0 - 360 degrees
            return (RadianToDegree(bearing) + 360) % 360;
        }

        // Calculate the destination point from given point having travelled the given distance (in km), on the given initial bearing (bearing may vary before destination is reached)
        private Location CalculateDestinationLocation(Location point, double bearing, double distance)
        {

            distance /= _earthRadius; // convert to angular distance in radians
            bearing = DegreeToRadian(bearing); // convert bearing in degrees to radians

            double lat1 = DegreeToRadian(point.latitude);
            double lon1 = DegreeToRadian(point.longitude);

            double lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(distance) + Math.Cos(lat1) * Math.Sin(distance) * Math.Cos(bearing));
            double lon2 = lon1 + Math.Atan2(Math.Sin(bearing) * Math.Sin(distance) * Math.Cos(lat1), Math.Cos(distance) - Math.Sin(lat1) * Math.Sin(lat2));
            lon2 = (lon2 + 3 * Math.PI) % (2 * Math.PI) - Math.PI; // normalize to -180 - + 180 degrees

            return new Location { latitude = RadianToDegree(lat2), longitude = RadianToDegree(lon2) };
        }

        // Calculate the distance between two points in km
        private double CalculateDistanceBetweenLocations(Location startPoint, Location endPoint)
        {

            double lat1 = DegreeToRadian(startPoint.latitude);
            double lon1 = DegreeToRadian(startPoint.longitude);

            double lat2 = DegreeToRadian(endPoint.latitude);
            double lon2 = DegreeToRadian(endPoint.longitude);

            double deltaLat = lat2 - lat1;
            double deltaLon = lon2 - lon1;

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return (_earthRadius * c);
        }
    }
}
