using Blog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Data
{
    public class Article
    {
        public Article()
        {
            this.AddedTime = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        [Column(TypeName = "text")]
        public string Content { get; set; }

        public DateTime AddedTime { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [NotMapped]
        public string Summury {
            get
            {
                if (this.Content == null)
                {
                    return null;
                }

                string summury = this.Content.Substring(0, this.Content.Length / 2) + "...";

                return summury;
            } 
                }
    }
}
