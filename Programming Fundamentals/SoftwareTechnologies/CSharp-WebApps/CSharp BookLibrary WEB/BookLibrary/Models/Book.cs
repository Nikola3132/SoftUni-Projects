using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Title { get; set; }
        [MinLength(10)]
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
    }
}