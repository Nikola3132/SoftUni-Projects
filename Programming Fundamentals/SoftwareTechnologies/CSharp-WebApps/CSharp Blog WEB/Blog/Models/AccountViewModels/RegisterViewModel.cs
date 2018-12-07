using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Blog.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Url]
        [Display(Name = "PhotoURL")]
        public string PhotoURL { get; set; }

        [Required]
        [Display(Name = "City/Village")]
        public string City { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }



        //protected void Upload(object sender, EventArgs e)
        //{
        //    if (PhotoURL.HasFile)
        //    {
        //        string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
        //        FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Images/") + fileName);
        //        Response.Redirect(Request.Url.AbsoluteUri);
        //    }
        //}
    }
}
