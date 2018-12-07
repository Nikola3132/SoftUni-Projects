using DItests.Core.Contracts;
using DItests.Models;
using DItests.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DItests.Core
{
    public class Engine:IEngine
    {
        IServiceProvider provider;
        public Engine(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public void Run()
        {
            //Already we have loaded services from the ctor. Theese services have reference to IPerson and we use it;
            IPerson person = (IPerson)provider.GetService(typeof(IPerson));
            //We already took the person and we can use all his methods and etc.
            person.SayHello();
            Console.WriteLine(person);
        }
    }
}
