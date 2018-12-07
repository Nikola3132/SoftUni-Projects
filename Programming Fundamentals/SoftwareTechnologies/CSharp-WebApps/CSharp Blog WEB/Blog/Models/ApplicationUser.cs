using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MinLength(5)]
        public string FullName { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Coutry { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string City { get; set; }
        [Required]
        public string PhotoURL { get; set; }

        public List<Article> Articles { get; set; }
    }
}
