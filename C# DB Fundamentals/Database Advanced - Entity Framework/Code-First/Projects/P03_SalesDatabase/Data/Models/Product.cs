using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P03_SalesDatabase.Data.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [Column("Id")]
        public int ProductId { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        
        [Required]
        [StringLength(250)]
        public string Description { get; set; }


        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
