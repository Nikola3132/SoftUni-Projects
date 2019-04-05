using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Prisoner")]
    public class ImportPrisonerDto
    {
        [XmlAttribute("id")]
        [Required]
        public string Id { get; set; }
    }
} 