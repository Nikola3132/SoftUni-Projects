using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    [Table("Countries")]
    public class Country
    {
        [Key]
        [Column("Id")]
        public int CountryId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Town> Towns { get; set; } = new List<Town>();
    }
}
