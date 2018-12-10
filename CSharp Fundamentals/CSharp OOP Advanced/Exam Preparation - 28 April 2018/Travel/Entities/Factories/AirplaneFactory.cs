namespace Travel.Entities.Factories
{
	using Contracts;
	using Airplanes.Contracts;
    using Travel.Entities.Airplanes;
    using System;
    using System.Reflection;
    using System.Linq;

    public class AirplaneFactory : IAirplaneFactory
	{
		public IAirplane CreateAirplane(string type)
		{
            Type typeOfAirplane = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(x => x.Name == type);

            return (IAirplane)(Activator.CreateInstance(typeOfAirplane, new object[] { }));
		}
	}
}