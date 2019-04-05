using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    [Table("Mails")]
    public class Mail
    {
        //Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //Description– text(required)
        [Required]
        public string Description { get; set; }

        //Sender – text(required)
        [Required]
        public string Sender { get; set; }

        //Address – text, consisting only of letters, spaces and numbers, which ends with “ str.” (required)
        [Required]
        [RegularExpression(@"^[A-z 0-9]+ str.$")]
        public string Address { get; set; }

        //PrisonerId - integer, foreign key
        [Required]
        public int PrisonerId { get; set; }

        //Prisoner – the mail's Prisoner (required)
        [ForeignKey("PrisonerId")]
        public Prisoner Prisoner { get; set; }

    }
}