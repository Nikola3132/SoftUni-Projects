using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetClinic.Models
{
    public class AnimalAid
    {
        [Key]
        public int Id { get; set; }
        //Id – integer, Primary Key

        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string Name { get; set; }
        //Name – text with min length 3 and max length 30 (required, unique)

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }
        //Price – decimal (non-negative, minimum value: 0.01, required)

        public ICollection<ProcedureAnimalAid> AnimalAidProcedures { get; set; } = new HashSet<ProcedureAnimalAid>();
        //AnimalAidProcedures – collection of type ProcedureAnimalAid

    }
}
