using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Data.Models
{
    public class Seat
    {
        //Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //HallId – integer, foreign key(required)
        [Required]
        [ForeignKey("Hall")]
        public int HallId { get; set; }

        //Hall – the seat’s hall
        public Hall Hall { get; set; }

    }
}