﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    [Table("PlayerStatistics")]
    public class PlayerStatistic
    {
        [Required]
        public int GameId { get; set; }

        [ForeignKey("GameId")]
        public Game Game { get; set; }
        
        [Required]
        public int PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        [Required]
        public int ScoredGoals { get; set; }
        
        [Required]
        public int Assists { get; set; }

        [Required]
        public int MinutesPlayed { get; set; }
        
    }
}