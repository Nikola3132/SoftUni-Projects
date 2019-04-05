namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var depCells = JsonConvert.DeserializeObject<ImportDepartmentCells[]>(jsonString);

            List<Department> departments = new List<Department>();
            List<Cell> cells = new List<Cell>();

            StringBuilder sb = new StringBuilder();

            foreach (var depDto in depCells)
            {
                if (!IsValid(depDto) || !depDto.Cells.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var depChecker = departments.Any(d => d.Name == depDto.Name);
                Department department = null;

                if (depChecker == false)
                {
                    department = new Department()
                    {
                        Name = depDto.Name
                    };
                }
                else
                {
                    department = departments.FirstOrDefault(d => d.Name == depDto.Name);
                }

                foreach (var cellDto in depDto.Cells)
                {
                    var cellChecker = cells.Any(c => c.CellNumber == cellDto.CellNumber);

                    Cell cell = null;

                    if (cellChecker == false)
                    {
                        cell = new Cell()
                        {
                            CellNumber = cellDto.CellNumber,
                            HasWindow = cellDto.HasWindow
                        };
                        cells.Add(cell);
                    }
                    else
                    {
                        cell = cells.FirstOrDefault(c => c.CellNumber == cellDto.CellNumber);
                    }
                    department.Cells.Add(cell);
                }

                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Cells.AddRange(cells);
            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var prisonerDtos = JsonConvert.DeserializeObject<ImportPrisonerMailsDto[]>(jsonString);

            var prisoners = new List<Prisoner>();
            var mails = new List<Mail>();

            foreach (var prisonerDto in prisonerDtos)
            {
                if (!IsValid(prisonerDto) || !prisonerDto.Mails.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var prisoner = prisoners.FirstOrDefault(p => p.Nickname == prisonerDto.Nickname);
                
                if (DateTime.TryParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime res))
                {
                }

                if (prisoner == null)
                {
                    prisoner = new Prisoner()
                    {
                        Age = prisonerDto.Age,
                        Bail = prisonerDto.Bail,
                        CellId = prisonerDto.CellId,
                        FullName = prisonerDto.FullName,
                        IncarcerationDate = DateTime.ParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ReleaseDate = res,
                        Nickname = prisonerDto.Nickname
                    };
                }

                foreach (var mailDto in prisonerDto.Mails)
                {
                    var mail = mails.FirstOrDefault(m => m.Description == mailDto.Description);
                    if (mail == null)
                    {
                        mail = new Mail()
                        {
                            Address = mailDto.Address,
                            Sender = mailDto.Sender,
                            Description = mailDto.Description,
                            Prisoner = prisoner
                        };
                        mails.Add(mail);
                    }
                    prisoner.Mails.Add(mail);
                }

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }
            context.Prisoners.AddRange(prisoners);
            context.Mails.AddRange(mails);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ImportOfficerPrisoners[])
                , new XmlRootAttribute("Officers"));

            var officersDto = (ImportOfficerPrisoners[])serializer.Deserialize(new StringReader(xmlString));

            var officers = new List<Officer>();
            var officersPrisoners = new List<OfficerPrisoner>();

            foreach (var officerDto in officersDto)
            {
                if (!IsValid(officerDto) || !officerDto.Prisoners.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                var checkerOfficer = officers.Any(o => o.FullName == officerDto.FullName);

                Officer officer = null;

                if (checkerOfficer == false)
                {
                    bool positionChecker = Enum.TryParse<Position>(officerDto.Position, out Position posRes);
                    bool weaponChecker = Enum.TryParse<Weapon>(officerDto.Weapon, out Weapon wepRes);

                    if (!positionChecker || !weaponChecker)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    officer = new Officer()
                    {
                        FullName = officerDto.FullName,
                        DepartmentId = officerDto.DepartmentId,
                        Salary = officerDto.Salary,
                        Position = posRes,
                        Weapon = wepRes
                    };
                }
                else
                {
                   officer = officers.FirstOrDefault(o => o.FullName == officerDto.FullName);
                }

                foreach (var prisonerDto in officerDto.Prisoners)
                {
                       var  officerPrisoner = new OfficerPrisoner()
                        {
                            OfficerId = officer.Id,
                            PrisonerId = int.Parse(prisonerDto.Id)
                        };

                        
                    

                    officersPrisoners.Add(officerPrisoner);
                    officer.OfficerPrisoners.Add(officerPrisoner);
                }
                
                    officers.Add(officer);
               
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");

            }
            context.Officers.AddRange(officers);
            context.OfficersPrisoners.AddRange(officersPrisoners);

            context.SaveChanges();
            return sb.ToString();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            return isValid;
        }
    }
}