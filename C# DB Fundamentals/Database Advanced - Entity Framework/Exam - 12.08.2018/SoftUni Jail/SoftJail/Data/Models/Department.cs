using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    [Table("Departments")]
    public class Department
    {
        //Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //Name – text with min length 3 and max length 25 (required)
        [Required]
        [StringLength(maximumLength:25,MinimumLength =3)]
        public string Name { get; set; }

        //Cells - collection of type Cell
        public ICollection<Cell> Cells { get; set; } = new HashSet<Cell>();
    }
}