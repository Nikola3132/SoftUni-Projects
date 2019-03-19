using MyApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Models
{
    public class Pet
    {
        public PetEnum PetType { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}
