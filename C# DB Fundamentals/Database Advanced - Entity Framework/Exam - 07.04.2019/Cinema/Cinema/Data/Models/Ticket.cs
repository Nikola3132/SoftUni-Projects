using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Data.Models
{
    public class Ticket
    {
        //Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //Price – decimal (non-negative, minimum value: 0.01) (required)
        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        //CustomerId – integer, foreign key(required)
        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        //Customer – the customer’s ticket
        public Customer Customer { get; set; }

        //ProjectionId – integer, foreign key(required)
        [Required]
        [ForeignKey("Projection")]
        public int ProjectionId { get; set; }

        //Projection – the projection’s ticket
        public Projection Projection { get; set; }

    }
}