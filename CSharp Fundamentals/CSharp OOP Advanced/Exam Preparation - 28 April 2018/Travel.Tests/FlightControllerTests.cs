// REMOVE any "using" statements, which start with "Travel." BEFORE SUBMITTING

namespace Travel.Tests
{
    using NUnit.Framework;
    using System;
    using System.Linq;
    using Travel.Core.Controllers;
    using Travel.Entities;
    using Travel.Entities.Airplanes;
    using Travel.Entities.Items;

    [TestFixture]
    public class FlightControllerTests
    {
        [Test]
        public void TestIfTheFlightControllerInheritHisInterface()

        {
            Type type = typeof(FlightController);
            var inheritInterfaces = type.GetInterfaces();

            Assert.That(inheritInterfaces.Length, Is.EqualTo(1));
            var checkedInterface = inheritInterfaces.FirstOrDefault();

            Assert.That(checkedInterface.Name, Is.EqualTo("IFlightController"));
        }
        
        [Test]
        public void SuccessfulTrip()
        {
            var passengers = new[]
            {
                new Passenger("Pesho1"),
                new Passenger("Pesho2"),
                new Passenger("Pesho3"),
                new Passenger("Pesho4"),
                new Passenger("Pesho5"),
                new Passenger("Pesho6"),
            };

            var airplane = new LightAirplane();

            foreach (var passenger in passengers)
            {
                airplane.AddPassenger(passenger);
            }

            var trip = new Trip("Sofia", "London", airplane);

            var airport = new Airport();

            airport.AddTrip(trip);

            var flightController = new FlightController(airport);

            var bag = new Bag(passengers[1], new[] { new Colombian() });

            passengers[1].Bags.Add(bag);

            var completedTrip = new Trip("Sofia", "Varna", new LightAirplane());
            completedTrip.Complete();

            airport.AddTrip(completedTrip);

            var actualResult = flightController.TakeOff();

            var expectedResult =
                @"SofiaLondon1:
Overbooked! Ejected Pesho2
Confiscated 1 bags ($50000)
Successfully transported 5 passengers from Sofia to London.
Confiscated bags: 1 (1 items) => $50000";

            Assert.That(actualResult, Is.EqualTo(expectedResult).NoClip);
            Assert.That(trip.IsCompleted, Is.True);
        }

    }
}