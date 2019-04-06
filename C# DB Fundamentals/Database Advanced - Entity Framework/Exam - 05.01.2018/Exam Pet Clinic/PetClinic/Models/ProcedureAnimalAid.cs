using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Models
{
    public class ProcedureAnimalAid
    {

        public int ProcedureId { get; set; }
        //ProcedureId – integer, Primary Key

        [Required]
        [ForeignKey("ProcedureId")]
        public Procedure Procedure { get; set; }
        //Procedure – the animal aid’s procedure(required)

        public int AnimalAidId { get; set; }
        //AnimalAidId – integer, Primary Key

        [Required]
        [ForeignKey("AnimalAidId")]
        public AnimalAid AnimalAid { get; set; }
        //AnimalAid – the procedure’s animal aid(required)

    }
}