using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }
        //-	Id – integer, Primary Key

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Name { get; set; }
        //-	Name – text with min length 3 and max length 20 (required)

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Type { get; set; }
        //-	Type – text with min length 3 and max length 20 (required)

        [Required]
        [Range(1, int.MaxValue)]
        public int Age { get; set; }
        //-	Age – integer, cannot be negative or 0 (required)

        public string PassportSerialNumber { get; set; }
        //-	PassportSerialNumber ¬– string, foreign key

        [Required]
        [ForeignKey("PassportSerialNumber")]
        public Passport Passport { get; set; }
        //-	Passport – the passport of the animal(required)

        public ICollection<Procedure> Procedures { get; set; } = new HashSet<Procedure>();
        //-	Procedures – the procedures, performed on the animal

    }
}
