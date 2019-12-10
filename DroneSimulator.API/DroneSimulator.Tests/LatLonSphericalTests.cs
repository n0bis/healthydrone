using System;
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

            int magnitude = 1 + (expected == 0.0 ? -1 : Convert.ToInt32(Math.Floor(Math.Log10(expected))));
            int precision = 11 - magnitude;

            double tolerance = 1.0 / Math.Pow(10, precision);
            // Assert
            Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
        }
    }
}
