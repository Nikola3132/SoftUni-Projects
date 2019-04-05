namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {

            var prisoners = context.Prisoners.Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(o => new
                    {
                        OfficerName = o.Officer.FullName,
                        Department = o.Officer.Department.Name
                    })
                    .OrderBy(o => o.OfficerName)
                    .ToArray(),
                    TotalOfficerSalary = p.PrisonerOfficers.Select(o => o.Officer)
                    .Sum(o => o.Salary)
                }).OrderBy(p => p.Name).ThenBy(p => p.Id)
                .ToArray();

            var json = JsonConvert.SerializeObject(prisoners, Newtonsoft.Json.Formatting.Indented);

            return json;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prisonerNamesCollection = prisonersNames.Split(",");

            var prisoners = context.Prisoners.Where(p => prisonersNames.Contains(p.FullName))
                .ToArray()
                .Select(p => new ExportPrisonerDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails.Select(m => new ExportEncryptedMessagesDto
                    {
                        Description = new string(m.Description.Reverse().ToArray())
                    }).ToArray()
                }).ToArray().OrderBy(p => p.Name).ThenBy(p => p.Id)
                .ToArray();
                

            //for (int i = 0; i < prisoners.Length; i++)
            //{
            //    for (int j = 0; j < prisoners[i].EncryptedMessages.Length; j++)
            //    {
            //        prisoners[i].EncryptedMessages[j].Description
            //            = new string(prisoners[i].EncryptedMessages[j].Description.Reverse().ToArray());
            //    }
            //}
            
                
            

            var serializer = new XmlSerializer(typeof(ExportPrisonerDto[]), new XmlRootAttribute("Prisoners"));

            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            StringBuilder sb = new StringBuilder();
            serializer.Serialize(new StringWriter(sb), prisoners, namespaces);



            return sb.ToString();
        }
    }
}