using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.ImportDtos
{
    [XmlType("AnimalAid")]
    public class ImportProcAnimalAid
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string Name { get; set; }
    }
}