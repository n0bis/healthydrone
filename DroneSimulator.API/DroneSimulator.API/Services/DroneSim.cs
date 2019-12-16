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
    // note greek letters (e.g. φ, λ, θ) are used for angles in radians to distinguish from angles in
    // degrees (e.g. lat, lon, brng)
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

        private double DegreeToRadian(double degree)
        {
            return (degree * Math.PI / 180);
        }

        private double RadianToDegree(double radian)
        {
            return (radian * 180 / Math.PI);
        }

        /// <summary>
        /// Calculate the (initial) bearing between two points, in degrees
        /// formula is for the initial bearing (sometimes referred to as forward azimuth):
        /// θ = atan2( sin Δλ ⋅ cos φ2 , cos φ1 ⋅ sin φ2 − sin φ1 ⋅ cos φ2 ⋅ cos Δλ )
        /// where: φ1,λ1 is the start point, φ2,λ2 the end point (Δλ is the difference in longitude)
        /// </summary>
        /// <returns>The initial bearing from ‘this’ point to destination point.</returns>
        /// <see cref="http://www.movable-type.co.uk/scripts/latlong.html"/>
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

        /// <summary>
        /// Calculate the destination point from given point having travelled the given distance (in km), on the given initial bearing (bearing may vary before destination is reached)
        /// using the formula:
        /// φ2 = asin( sin φ1 ⋅ cos δ + cos φ1 ⋅ sin δ ⋅ cos θ )
        /// λ2 = λ1 + atan2(sin θ ⋅ sin δ ⋅ cos φ1, cos δ − sin φ1 ⋅ sin φ2)
        /// where: φ is latitude, λ is longitude, θ is the bearing (clockwise from north), δ is the angular distance d/R; d being the distance travelled, R the earth’s radius
        /// </summary>
        /// <param name="point"></param>
        /// <param name="bearing">Initial bearing in degrees from north.</param>
        /// <param name="distance">Distance travelled, in same units as earth radius</param>
        /// <returns>the destination point</returns>
        /// <see cref="http://www.movable-type.co.uk/scripts/latlong.html"/>
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

        /// <summary>
        /// Calculate the distance between two points in km
        /// using the Haversine formula:
        /// a = sin²(Δφ/2) + cos φ1 ⋅ cos φ2 ⋅ sin²(Δλ/2)
        /// c = 2 ⋅ atan2( √a, √(1−a) )
        /// d = R ⋅ c
        /// where: φ is latitude, λ is longitude, R is earth’s radius(mean radius = 6,371km);
        /// note that angles need to be in radians
        /// </summary>
        /// <returns>Distance between points "as the crow flies" in kilometers</returns>
        /// <see cref="http://www.movable-type.co.uk/scripts/latlong.html"/>
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
