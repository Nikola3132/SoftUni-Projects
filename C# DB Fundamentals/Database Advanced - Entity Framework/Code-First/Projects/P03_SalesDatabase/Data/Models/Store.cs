using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P03_SalesDatabase.Data.Models
{
    [Table("Stores")]
    public class Store
    {
        [Key]
        [Column("Id")]
        public int StoreId { get; set; }

        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }

}
