using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    //, , , , 
    [Table("Teams")]
    public class Team
    {
        [Key]
        [Column("Id")]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        [Url]
        public string LogoUrl { get; set; }

        [Required]
        [StringLength(20)]
        public string Initials { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(15,2)")]
        public decimal Budget { get; set; }

        [Required]
        public int PrimaryKitColorId { get; set; }

        [ForeignKey("PrimaryKitColorId")]
        public Color PrimaryKitColor { get; set; }

        [Required]
        public int SecondaryKitColorId { get; set; }

        [ForeignKey("SecondaryKitColorId")]
        public Color SecondaryKitColor { get; set; }

        [Required]
        public int TownId { get; set; }

        [ForeignKey("TownId")]
        public Town Town { get; set; }

        public ICollection<Player> Players { get; set; } = new List<Player>();

        public ICollection<Game> HomeGames { get; set; } = new List<Game>();

        public ICollection<Game> AwayGames { get; set; } = new List<Game>();
    }
}
