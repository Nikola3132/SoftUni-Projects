using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Models
{
    public class Student
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public int StudentId { get; set; }

        public ICollection<Pet> Pets { get; set; } = new List<Pet>();
    }
}
