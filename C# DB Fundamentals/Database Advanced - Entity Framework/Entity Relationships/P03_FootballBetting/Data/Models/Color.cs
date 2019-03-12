using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    [Table("Colors")]
    public class Color
    {
        [Key]
        [Column("Id")]
        public int ColorId { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public ICollection<Team> PrimaryKitTeams { get; set; } = new List<Team>();

        public ICollection<Team> SecondaryKitTeams { get; set; } = new List<Team>();

    }
}
