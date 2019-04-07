using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Customer")]
    public class ImportCustomerDto
    {
        [XmlElement("FirstName")]
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [XmlElement("LastName")]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [XmlElement("Age")]
        [Range(12, 110)]
        public int Age { get; set; }

        [Required]
        [XmlElement("Balance")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Balance { get; set; }

        [XmlArray("Tickets")]
        public ImportTicketDto[] Tickets { get; set; }

    }
}
