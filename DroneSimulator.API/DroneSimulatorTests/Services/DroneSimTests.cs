using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneSimulator.API.Services;
using System;
using System.Collections.Generic;
using System.Text;
using DroneSimulator;
using DroneSimulator.API.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace DroneSimulator.API.Services.Tests
{
    [TestFixture]
    public class DroneSimTests
    {
        ServiceProvider _provider;
        DroneSim _droneSim;
        
        [OneTimeSetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddTransient<IOptions<DroneOpts>>(
                provider => Options.Create<DroneOpts>(new DroneOpts
                {
                    id = "et id",
                    operationid = "et id mere",
                    velocity = 10.0,
                    homelocation = new Location
                    {
                        latitude = 55.39594,
                        longitude = 10.38831
                    },
                    location = new Location
                    {
                        latitude = 10.203921,
                        longitude = 56.162939
                    }
                }));

            services.AddTransient<IOptions<UTMOpts>>(
                provider => Options.Create<UTMOpts>(new UTMOpts
                {
                    clientid = "endnu et",
                    clientsecret = "gg",
                    username = "mafal",
                    password = "maf"
                }));

            services.AddTransient<DroneSim>();
            _provider = services.BuildServiceProvider();
            _droneSim = _provider.GetService<DroneSim>();
        }

        [Test]
        public void DegreeToRadianTest()
        {
            double expected = 0.78539816339;
            double actual = _droneSim.DegreeToRadian(45.0);
            Assert.AreEqual(actual, expected);
        }
/*
        [TestMethod()]
        public void RadianToDegreeTest()
        {
            throw new NotImplementedException();
        }
        [TestMethod()]
        public void CalculateBearingTest(Location startPoint, Location endPoint)
        {
            throw new NotImplementedException();
        }
        [TestMethod()]
        public Location CalculateDestinationLocationTest(Location point, double bearing, double distance)
        {
            throw new NotImplementedException();
        }
        [TestMethod()]
        public void CalculateDistanceBetweenLocationsTest(Location startPoint, Location endPoint)
        {
            throw new NotImplementedException();
        }
*/
    }
}