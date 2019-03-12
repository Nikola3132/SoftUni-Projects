using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P03_FootballBetting.Data.Models
{

    // , , IsInjured
    [Table("Players")]
    public class Player
    {
        [Key]
        [Column("Id")]
        public int PlayerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int SquadNumber { get; set; }

        [Required]
        public int TeamId { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public bool IsInjured { get; set; }

        [ForeignKey("PositionId")]
        public Position Position { get; set; }

        public ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new List<PlayerStatistic>();
    }
}
