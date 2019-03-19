using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Models
{
    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Pet> Pets { get; set; } = new List<Pet>();
    }
}
