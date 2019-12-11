using System;
using DroneSimulator.API.Domain.Models;
using DroneSimulator.API.Helpers;
using NUnit.Framework;

namespace DroneSimulator.Tests
{
    [TestFixture]
    public class LatLonSphericalTests
    {
        [Test]
        public void DegreeToRadianTest_Success()
        {
            // Arrange
            double expected = 0.78539816339;

            // Act
            double actual = LatLonSpherical.DegreeToRadian(45.0);

            double tolerance = calculateTolerance(expected);
            // Assert
            Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
        }
       
        [Test]
        public void RadianToDegreeTest_Succes()
        {
            //Arrange
            double expected = 34.3774677078;
            //Act
            double actual = LatLonSpherical.RadianToDegree(0.60);

            double tolerance = calculateTolerance(expected);
            //Assert
            Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
        }

        [Test]
        public void CalculateBearing()
        {
            //Arrange
            double expected = 284.00119698271232;
            Location startLocation = new Location { latitude = 59.571111, longitude = 4.133889 };
            Location endLocation = new Location { latitude = 42.351111, longitude = -71.040833 };
            //Act
            double actual = LatLonSpherical.CalculateBearing(startLocation, endLocation);
            //Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CalculateDestinationLocation()
        {
            //Arrange
            Location expected = new Location { latitude = 53.188333, longitude = 0.133333 };
            Location point = new Location { latitude = 53.320556, longitude = -1.729722 };
            double bearing = 96.018254433006632;
            double distance = 124.8;
            //Act
            Location actual = LatLonSpherical.CalculateDestinationLocation(point, bearing, distance);

            //Assert
            Assert.That(actual, Has.Property("latitude").EqualTo(expected.latitude).Within(0.0005) 
                            & Has.Property("longitude").EqualTo(expected.longitude).Within(0.0005));
        }

        [Test]
        public void CalculateDistanceBetweenLocations()
        {
            //Arrange
            double expected = 124.8;
            Location startLocation = new Location { latitude = 53.188333, longitude = 0.133333 };
            Location endLocation = new Location { latitude = 53.320556, longitude = -1.729722 };
            //Act
            double actual = LatLonSpherical.CalculateDistanceBetweenLocations(startLocation, endLocation);
            //Assert
            Assert.That(actual, Is.EqualTo(expected).Within(0.05));
        }


        private double calculateTolerance(double expected, int digits = 11)
        {
            int magnitude = 1 + (expected == 0.0 ? -1 : Convert.ToInt32(Math.Floor(Math.Log10(expected))));
            int precision = digits - magnitude;

            return (1.0 / Math.Pow(10, precision));
        }

       
    }
}
