using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    [Table("Diagnoses")]
    public class Diagnose
    {
        [Key]
        [Required]
        [Column("Id")]
        public int DiagnoseId { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        public string Comments { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        public Patient Patient { get; set; }
    }
}
