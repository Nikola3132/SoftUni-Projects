using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetClinic.DataProcessor.ImportDtos
{
    public class ImportAnimalDto
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Name { get; set; }


        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Type { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Age { get; set; }

        public ImportPassportDto Passport { get; set; }
        //      {
        //  "Name":"Bella",
        //  "Type":"cat",
        //  "Age": 2,
        //  "Passport": {
        //    "SerialNumber": "etyhGgH678",
        //    "OwnerName": "Sheldon Cooper",
        //    "OwnerPhoneNumber": "0897556446",
        //    "RegistrationDate": "12-03-2014"
        //  }
        //}

    }
}
