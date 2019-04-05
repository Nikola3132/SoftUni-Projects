using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentCells
    {
        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 3)]
        public string Name { get; set; }

        public ImportCellDto[] Cells { get; set; }
        
    }
}
