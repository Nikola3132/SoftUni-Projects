﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetClinic.Models
{
    public class Vet
    {
        [Key]
        public int Id { get; set; }
        //Id – integer, Primary Key

        [Required]
        [StringLength(maximumLength: 40, MinimumLength = 3)]
        public string Name { get; set; }
        //Name – text with min length 3 and max length 40 (required)

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public string Profession { get; set; }
        //Profession – text with min length 3 and max length 50 (required)

        [Required]
        [Range(22, 65)]
        public int Age { get; set; }
        //Age – integer number, minimum value of 22 years and maximum 65 (required)

        [Required]
        [RegularExpression(@"^\+359[0-9]{9}$|^0[0-9]{9}$")]
        public string PhoneNumber { get; set; }
        //PhoneNumber – required, unique, make sure it matches one of the following requirements:
        //either starts with +359 and is followed by 9 digits
        //or consists of exactly 10 digits, starting with 0

        public ICollection<Procedure> Procedures { get; set; } = new HashSet<Procedure>();
        //Procedures – the procedures, performed by the vet
    }
}