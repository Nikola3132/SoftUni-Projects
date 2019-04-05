using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonerMailsDto
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^The [A-Z][a-z]+$")]
        public string Nickname { get; set; }

        [Required]
        [Range(18, 65)]
        public int Age { get; set; }

        [Required]
        public string IncarcerationDate { get; set; }
        

        public string ReleaseDate { get; set; }

        [Range(typeof(decimal), "0.00", "79228162514264337593543950335")]
        public decimal? Bail { get; set; }

        [Required]
        public int CellId { get; set; }

        public ImportMailDto[] Mails { get; set; }
        
    }
}
