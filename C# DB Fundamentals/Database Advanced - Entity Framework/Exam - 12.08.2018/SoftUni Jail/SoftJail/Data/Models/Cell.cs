using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    [Table("Cells")]
    public class Cell
    {
        //Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //CellNumber – integer in the range[1, 1000] (required)
        [Required]
        [Range(1,1000)]
        public int CellNumber { get; set; }

        //HasWindow – bool (required)
        [Required]
        public bool HasWindow { get; set; }

        //DepartmentId - integer, foreign key
        [Required]
        public int DepartmentId { get; set; }

        //Department – the cell's department (required)
        
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        //Prisoners - collection of type Prisoner
        public ICollection<Prisoner> Prisoners { get; set; } = new HashSet<Prisoner>();

    }
}