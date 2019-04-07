using Cinema.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.DataProcessor.ImportDto
{
    public class ImportMovieDto
    {
        //"Title": "Gui Si (Silk)",
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Title { get; set; }

        // "Genre": "Drama",
        [Required]
        public Genre Genre { get; set; }

        // "Duration": "02:21:00",
        [Required]
        public TimeSpan Duration { get; set; }

        // "Rating": 9,
        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }

        // "Director": "Perl Swyne"
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Director { get; set; }
    }
}
