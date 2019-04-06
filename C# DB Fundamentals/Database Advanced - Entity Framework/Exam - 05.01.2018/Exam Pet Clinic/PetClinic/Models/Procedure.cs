using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PetClinic.Models
{
    public class Procedure
    {
        [Key]
        public int Id { get; set; }
        //Id – integer, Primary Key

        public int AnimalId { get; set; }
        //AnimalId ¬– integer, foreign key

        [ForeignKey("AnimalId")]
        [Required]
        public Animal Animal { get; set; }
        //Animal – the animal on which the procedure is performed(required)

        public int VetId { get; set; }
        //VetId ¬– integer, foreign key

        [ForeignKey("VetId")]
        [Required]
        public Vet Vet { get; set; }
        //Vet – the clinic’s employed doctor servicing the patient(required)

        public ICollection<ProcedureAnimalAid> ProcedureAnimalAids { get; set; } = new HashSet<ProcedureAnimalAid>();
        //ProcedureAnimalAids – collection of type ProcedureAnimalAid

        [NotMapped]
        public decimal Cost => this.ProcedureAnimalAids.Sum(pa => pa.AnimalAid.Price);
        //Cost – the cost of the procedure, calculated by summing the price of the different services performed;
        //        does not need to be inserted in the database

        [Required]
        public DateTime DateTime { get; set; }
        //DateTime – the date and time on which the given procedure is performed(required)

    }
}