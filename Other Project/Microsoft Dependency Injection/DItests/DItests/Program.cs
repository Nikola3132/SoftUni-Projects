using DItests.Core;
using DItests.Core.Contracts;
using DItests.Models;
using DItests.Models.Contracts;
using DItests.Models.People;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DItests
{
    public class Program
    {
        public  static void Main(string[] args)
        {
            //Configuring all Interfaces and Classes to be mapped each other from custom method ServiceConfiguration();
            IServiceProvider provider = ServiceConfiguration();

            //we are calling the engine from the services;
           var engine = provider.GetService<IEngine>();
            engine.Run();
        }

        private static IServiceProvider ServiceConfiguration()
        {
            //Creating IServiceCollection to collect all services we need
            IServiceCollection services = new ServiceCollection();

            //AddTransient will create an instance every time we call the interface mapped with the given class

            services.AddTransient<IEngine, Engine>();
            services.AddTransient<IPerson, Programmer>();
            services.AddTransient<IJob, Job>();
            services.AddTransient<IPassport, Passport>();

            //This method combine all services in one
            IServiceProvider readyProvider = services.BuildServiceProvider();
            return readyProvider;
        }
    }
}
