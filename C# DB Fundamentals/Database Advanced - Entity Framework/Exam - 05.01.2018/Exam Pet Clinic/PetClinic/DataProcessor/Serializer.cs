namespace PetClinic.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.ExportDtos;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var animals = context.Animals
                .Where(a => a.Passport.OwnerPhoneNumber == phoneNumber)
                .Select(a => new
                {
                    OwnerName = a.Passport.OwnerName,
                    AnimalName = a.Name,
                    Age = a.Age,
                    SerialNumber = a.PassportSerialNumber,
                    RegisteredOn = a.Passport.RegistrationDate.ToString("dd-MM-yyyy",CultureInfo.InvariantCulture)
                })
                .OrderBy(a=>a.Age)
                .ThenBy(a=>a.SerialNumber)
                .ToArray();

            return JsonConvert.SerializeObject(animals, Newtonsoft.Json.Formatting.Indented); 
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            var procs = context.Procedures
                .Select(p => new ExportProcDto
                {
                    DateTime = p.DateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    OwnerPhoneNumber = p.Animal.Passport.OwnerName,
                    SerialNumber = p.Animal.PassportSerialNumber,
                    TotalPrice = p.ProcedureAnimalAids.Sum(j => j.AnimalAid.Price),
                    AnimalAids = p.ProcedureAnimalAids.Select(e=>e.AnimalAid).Select(e=> new ExportAnimalAidDto
                    {
                        Name = e.Name,
                        Price = e.Price
                    }).ToArray()
                }).OrderBy(e=>e.DateTime)
                .ThenBy(e=>e.SerialNumber)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportProcDto[]), 
                new XmlRootAttribute("Procedures"));


            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            StringBuilder sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), procs, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
