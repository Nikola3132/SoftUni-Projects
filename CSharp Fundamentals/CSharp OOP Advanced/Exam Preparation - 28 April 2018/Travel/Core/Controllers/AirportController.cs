namespace Travel.Core.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Contracts;
	using Entities;
	using Entities.Contracts;
	using Entities.Factories;
	using Entities.Factories.Contracts;
    using Travel.Entities.Airplanes.Contracts;
    using Travel.Entities.Items.Contracts;

    public class AirportController : IAirportController
    {
        private const int BagValueConfiscationThreshold = 3000;

        private IAirport airport;

        private IAirplaneFactory airplaneFactory;
        private IItemFactory itemFactory;

        public AirportController(IAirport airport)
        {
            this.airport = airport;
            this.airplaneFactory = new AirplaneFactory();
            this.itemFactory = new ItemFactory();
        }
        public string RegisterPassenger(string username)
        {
            IPassenger passenger = this.airport.GetPassenger(username);
            if (passenger != null)
            {
                throw new InvalidOperationException($"Passenger {username} already registered!");
            }

            passenger = new Passenger(username);

            this.airport.AddPassenger(passenger);
            return $"Registered {passenger.Username}";
        }

        public string RegisterTrip(string source, string destination, string planeType)
        {
            IAirplane airplane = this.airplaneFactory.CreateAirplane(planeType);

            ITrip trip = new Trip(source, destination, airplane);

            this.airport.AddTrip(trip);

            return $"Registered trip {trip.Id}";
        }

        public string RegisterBag(string username, IEnumerable<string> bagItems)
        {
            var passenger = this.airport.GetPassenger(username);

            IEnumerable<IItem> items = bagItems.Select(x => itemFactory.CreateItem(x));
            var bag = new Bag(passenger, items);

            passenger.Bags.Add(bag);

            return $"Registered bag with {string.Join(", ", bagItems)} for {username}";
        }


        public string CheckIn(string username, string tripId, IEnumerable<int> bagIndexes)
        {
            IPassenger passenger = this.airport.GetPassenger(username);
            ITrip trip = this.airport.GetTrip(tripId);

            if (trip.Airplane.Passengers.Contains(passenger))
            {
                throw new InvalidOperationException($"{username} is already checked in!");
            }

            var confiscatedBags = CheckInBags(passenger, bagIndexes);
            trip.Airplane.AddPassenger(passenger);

            return
                $"Checked in {passenger.Username} with {bagIndexes.Count() - confiscatedBags}/{bagIndexes.Count()} checked in bags";
        }

        private int CheckInBags(IPassenger passenger, IEnumerable<int> bagsToCheckIn)
        {
            int confiscatedBagCount = 0;
            foreach (var i in bagsToCheckIn)
            {
                var currentBag = passenger.Bags[i];
                passenger.Bags.RemoveAt(i);

                if (ShouldConfiscate(currentBag))
                {
                    airport.AddConfiscatedBag(currentBag);
                    confiscatedBagCount++;
                }
                else
                {
                    this.airport.AddCheckedBag(currentBag);
                }
            }

            return confiscatedBagCount;
        }

        private static bool ShouldConfiscate(IBag bag)
        {
            var luggageValue = 0;
            IItem[] bagItems = bag.Items.ToArray();

            for (int i = 0; i < bagItems.Length; i++)
            {
                luggageValue += bagItems[i].Value;
            }

            var shouldConfiscate = luggageValue > BagValueConfiscationThreshold;
            return shouldConfiscate;
        }
    }
}