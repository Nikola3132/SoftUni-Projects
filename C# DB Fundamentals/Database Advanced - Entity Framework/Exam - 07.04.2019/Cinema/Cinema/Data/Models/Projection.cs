using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Data.Models
{
    public class Projection
    {
        //Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //MovieId – integer, foreign key(required)
        [ForeignKey("Movie")]
        [Required]
        public int MovieId { get; set; }

        //Movie – the projection’s movie
        public Movie Movie { get; set; }

        //HallId – integer, foreign key(required)
        [Required]
        [ForeignKey("Hall")]
        public int HallId { get; set; }

        //Hall – the projection’s hall
        public Hall Hall { get; set; }

        //DateTime - DateTime(required)
        [Required]
        public DateTime DateTime { get; set; }

        //Tickets - collection of type Ticket
        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }
}