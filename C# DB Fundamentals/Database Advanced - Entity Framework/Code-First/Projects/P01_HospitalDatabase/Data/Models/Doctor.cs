using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace P01_HospitalDatabase.Data.Models
{
    [Table("Doctors")]
    public class Doctor
    {
        private string password;

        [Key]
        [Required]
        [Column("Id")]
        public int DoctorId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(160)]
        public string Specialty { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        [MinLength(5)]
        public string Password { get { return password; } set
            {
                var hash = SecurityPasswordHasher.Hash(value);
                    password = hash;
                
            } }

        public ICollection<Visitation> Visitations { get; set; } = new List<Visitation>();
        
    }
}
