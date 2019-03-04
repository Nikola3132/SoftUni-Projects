using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P03_SalesDatabase.Data.Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [Column("Id")]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}",ErrorMessage = "The emailAdress is not valid")]
        public string Email { get; set; }

        [StringLength(13)]
        public string CreditCardNumber { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
