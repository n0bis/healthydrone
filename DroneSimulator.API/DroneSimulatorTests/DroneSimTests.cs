using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneSimulator;
using DroneSimulator.API.Domain.Models;

namespace DroneSimulatorTests
{
    [TestClass]
    public class DroneSimTests
    {
        [TestMethod]
        private double DegreeToRadian(double degree)
        {
            return 2.2;
        }
        [TestMethod]
        private double RadianToDegree(double radian)
        {
            return 2.2;
        }
        [TestMethod]
        private double CalculateBearing(Location startPoint, Location endPoint)
        {
            return 2.2;
        }
        [TestMethod]
        private Location CalculateDestinationLocation(Location point, double bearing, double distance)
        {

            return null;
        }
        [TestMethod]
        private double CalculateDistanceBetweenLocations(Location startPoint, Location endPoint)
        {
            return 2.2;
        }
    }
}
