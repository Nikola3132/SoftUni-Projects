using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.Data.Models
{
    public class Hall
    {
        //Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //Name – text with length[3, 20] (required)
        [Required]
        [StringLength(maximumLength:20,MinimumLength =3)]
        public string Name { get; set; }

        //Is4Dx - bool
        [Required]
        public bool Is4Dx { get; set; }

        //Is3D - bool
        [Required]
        public bool Is3D { get; set; }

        //Projections - collection of type Projection
        public ICollection<Projection> Projections { get; set; } = new HashSet<Projection>();

        //Seats - collection of type Seat
        public HashSet<Seat> Seats { get; set; } = new HashSet<Seat>();

    }
}
