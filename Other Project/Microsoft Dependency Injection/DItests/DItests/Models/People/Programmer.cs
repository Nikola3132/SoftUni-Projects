namespace DItests.Models.People
{
    using System;

    using Contracts;

    public class Programmer : IPerson
    {
        
        private IServiceProvider serviceProvider;

        //From the getService() method it's automatically send the used services [ Example: provider.GetService(typeof(IPerson)) ] provider 
                          //are the used services and when we use the method GetServices() they will resend to the type ctor. Thats why we need ti accept IServiceProvider and save them on field for reusing!

        public Programmer(IServiceProvider serviceProvider)
        {
            
            this.serviceProvider = serviceProvider;

            this.Passport = (IPassport)this.serviceProvider.GetService(typeof(IPassport));
            this.Job = (IJob)this.serviceProvider.GetService(typeof(IJob));
        }
        
        public IPassport Passport { get; }

        public IJob Job { get; }

        public void SayHello()
        {
            Console.WriteLine("Hello World!");
        }

        public string ShowThePersonInfo()
        {
            return this.ToString();
        }

        public override string ToString()
        {
            return $"{this.Passport}{Environment.NewLine} {this.Job}";
        }
    }
}
