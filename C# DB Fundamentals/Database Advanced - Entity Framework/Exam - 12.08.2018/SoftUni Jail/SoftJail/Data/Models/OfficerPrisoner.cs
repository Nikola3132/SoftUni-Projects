using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    [Table("OfficersPrisoners")]
    public class OfficerPrisoner
    {
        //PrisonerId – integer, Primary Key
        
        [Required]
        public int PrisonerId { get; set; }

        //Prisoner – the officer’s prisoner(required)
       
        [ForeignKey("PrisonerId")]
        public Prisoner Prisoner { get; set; }

        //OfficerId – integer, Primary Key
        
        [Required]
        public int OfficerId { get; set; }

        //Officer – the prisoner’s officer(required)
       
        [ForeignKey("OfficerId")]
        public Officer Officer { get; set; }
    }
}