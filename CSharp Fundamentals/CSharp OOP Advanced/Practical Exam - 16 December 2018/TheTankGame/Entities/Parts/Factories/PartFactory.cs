using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TheTankGame.Entities.Parts.Contracts;
using TheTankGame.Entities.Parts.Factories.Contracts;

namespace TheTankGame.Entities.Parts.Factories
{
    public class PartFactory : IPartFactory
    {
        //string model, double weight, decimal price
        public IPart CreatePart(string partType, string model, double weight, decimal price, int additionalParameter)
        {
            if (partType .Contains("Part") == false)
            {
                partType += "Part";
            }
            Type type = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(x => x.Name == partType);

            IPart part = (IPart)Activator.CreateInstance(type, new object[] { model, weight, price, additionalParameter });

            return part;
        }
    }
}
