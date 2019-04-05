using SoftJail.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SoftJail.Data.Models
{
    [Table("Officers")]
    public class Officer
    {
        //Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //FullName – text with min length 3 and max length 30 (required)
        [Required]
        [StringLength(maximumLength:30,MinimumLength =3)]
        public string FullName { get; set; }

        //Salary – decimal (non-negative, minimum value: 0) (required)
        [Required]
        [Range(typeof(decimal),"0.00","79228162514264337593543950335")]
        public decimal Salary { get; set; }

        //Position - Position enumeration with possible values: “Overseer, Guard, Watcher, Labour” (required)
        [Required]
        public Position Position { get; set; }

        //Weapon - Weapon enumeration with possible values: “Knife, FlashPulse, ChainRifle, Pistol, Sniper”(required)
        [Required]
        public Weapon Weapon { get; set; }

        //DepartmentId - integer, foreign key
        
        [Required]
        public int DepartmentId { get; set; }

        //Department – the officer's department (required)
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        //OfficerPrisoners - collection of type OfficerPrisoner
        public ICollection<OfficerPrisoner> OfficerPrisoners { get; set; } = new HashSet<OfficerPrisoner>();

    }
}
