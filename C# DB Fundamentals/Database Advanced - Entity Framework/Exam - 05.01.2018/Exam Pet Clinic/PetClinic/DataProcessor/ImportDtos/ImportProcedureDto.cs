using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.ImportDtos
{
    [XmlType("Procedure")]
    public class ImportProcedureDto
    {
        [XmlElement("Vet")]
        [Required]
        [StringLength(maximumLength: 40, MinimumLength = 3)]
        public string VetName { get; set; }

        [XmlElement("Animal")]
        public string AnimalSerialNumber { get; set; }

        [Required]
        [XmlElement("DateTime")]
        public string DateTime { get; set; }

        [XmlArray("AnimalAids")]
        public ImportProcAnimalAid[] AnimalAids { get; set; }
     
    }
}
