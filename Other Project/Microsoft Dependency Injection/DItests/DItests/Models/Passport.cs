using DItests.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DItests.Models
{
    public class Passport : IPassport
    {
        private const string name = "Pesho";
        private const int age = 22;
        private const string personalID = "00423535";
        private const string birthPlace = "Sofia";
        
        private IServiceProvider serviceProvider;
        public Passport(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            this.Name = name;
            this.Age = age;
            this.PersonalPin = personalID;
            this.BirthPlace = birthPlace;
        }

        public string PersonalPin { get; }
        public string BirthPlace { get; }
        public string Name { get; }
        public int Age { get; }

        public override string ToString()
        {
            return $"Name: {this.Name}{Environment.NewLine}Age: {this.Age}{Environment.NewLine} {Environment.NewLine}Pin:{Environment.NewLine}      PersonalPin:{this.PersonalPin}{Environment.NewLine}      BirthPlace:{this.BirthPlace}{Environment.NewLine}";
        }
    }
}
