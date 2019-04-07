using Cinema.Data.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Movie
    {
        //Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //Title – text with length[3, 20] (required)
        [Required]
        [StringLength(maximumLength:20,MinimumLength =3)]
        public string Title { get; set; }

        //Genre – enumeration of type Genre, with possible values
        //(Action, Drama, Comedy, Crime, Western, Romance, Documentary, Children, Animation, Musical)
        //(required)
        [Required]
        public Genre Genre { get; set; }

        //Duration – TimeSpan(required)
        [Required]
        public TimeSpan Duration { get; set; }

        //Rating – double in the range[1, 10] (required)
        [Required]
        [Range(1,10)]
        public double Rating { get; set; }

        //Director – text with length[3, 20] (required)
        [Required]
        [StringLength(maximumLength:20,MinimumLength =3)]
        public string Director { get; set; }

        //Projections - collection of type Projection
        public ICollection<Projection> Projections { get; set; } = new HashSet<Projection>();


    }
}
