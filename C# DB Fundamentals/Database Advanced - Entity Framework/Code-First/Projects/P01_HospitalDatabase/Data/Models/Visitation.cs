using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_HospitalDatabase.Data.Models
{
    [Table("Visitations")]
    public class Visitation
    {
        [Column("Id")]
        [Key]
        [Required]
        public int VisitationId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(250)]
        public string Comments { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        [Required]
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; }
    }
}
