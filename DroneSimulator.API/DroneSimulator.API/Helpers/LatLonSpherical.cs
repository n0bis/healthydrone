using System;
using DroneSimulator.API.Domain.Models;

namespace DroneSimulator.API.Helpers
{
    public static class LatLonSpherical
    {
        private static double _earthRadius = 6371; // radius in km

        public static double DegreeToRadian(double degree)
        {
            return (degree * Math.PI / 180);
        }

        public static double RadianToDegree(double radian)
        {
            return (radian * 180 / Math.PI);
        }

        // Calculate the (initial) bearing between two points, in degrees
        public static double CalculateBearing(Location startPoint, Location endPoint)
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
        public static Location CalculateDestinationLocation(Location point, double bearing, double distance)
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
        public static double CalculateDistanceBetweenLocations(Location startPoint, Location endPoint)
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
