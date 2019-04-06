namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.ImportDtos;
    using PetClinic.Models;

    public class Deserializer
    {

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            var animalAidDtos = JsonConvert.DeserializeObject<ImportAnimalAidDto[]>(jsonString);

            List<AnimalAid> animalAids = new List<AnimalAid>();
            StringBuilder sb = new StringBuilder();

            foreach (var animalAidDto in animalAidDtos)
            {
                if (!IsValid(animalAidDto))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var animalAid = animalAids.SingleOrDefault(a => a.Name == animalAidDto.Name);

                if (animalAid == null)
                {
                    animalAid = new AnimalAid()
                    {
                        Name = animalAidDto.Name,
                        Price = animalAidDto.Price
                    };

                    animalAids.Add(animalAid);
                    sb.AppendLine($"Record {animalAid.Name} successfully imported.");
                }
                else
                {
                    sb.AppendLine("Error: Invalid data.");
                }
            }
            context.AnimalAids.AddRange(animalAids);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var animalDtos = JsonConvert.DeserializeObject<ImportAnimalDto[]>(jsonString);

            List<Animal> animals = new List<Animal>();
            List<Passport> passports = new List<Passport>();

            foreach (var animalDto in animalDtos)
            {
                if (!IsValid(animalDto) || !IsValid(animalDto.Passport))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var passport = passports.SingleOrDefault(p => p.SerialNumber == animalDto.Passport.SerialNumber);

                if (passport == null)
                {
                    passport = new Passport()
                    {
                        SerialNumber = animalDto.Passport.SerialNumber,
                        OwnerName = animalDto.Passport.OwnerName,
                        OwnerPhoneNumber = animalDto.Passport.OwnerPhoneNumber,
                        RegistrationDate 
                        = DateTime.ParseExact(animalDto.Passport.RegistrationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                    };

                    passports.Add(passport);
                }
                else
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var animal = new Animal()
                {
                    Age = animalDto.Age,
                    Name = animalDto.Name,
                    Passport = passport,
                    PassportSerialNumber = passport.SerialNumber,
                    Type = animalDto.Type
                };
                animals.Add(animal);

                sb.AppendLine($"Record {animal.Name} Passport №: {animal.Passport.SerialNumber} successfully imported.");
            }
            context.Animals.AddRange(animals);
            context.Passports.AddRange(passports);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
         }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ImportVetDto[])
                , new XmlRootAttribute("Vets"));

            var vetDtos = (ImportVetDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Vet> vets = new List<Vet>();

            foreach (var vetDto in vetDtos)
            {
                if (!IsValid(vetDto))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var vet = vets.SingleOrDefault(v => v.PhoneNumber == vetDto.PhoneNumber);

                if (vet == null)
                {
                    vet = new Vet()
                    {
                        Age = vetDto.Age,
                        Name = vetDto.Name,
                        PhoneNumber = vetDto.PhoneNumber,
                        Profession = vetDto.Profession
                    };
                    vets.Add(vet);
                    sb.AppendLine($"Record {vet.Name} successfully imported.");
                }
                else
                {
                    sb.AppendLine("Error: Invalid data.");
                }

            }
                context.Vets.AddRange(vets);
                context.SaveChanges();

                return sb.ToString().TrimEnd();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ImportProcedureDto[])
                , new XmlRootAttribute("Procedures"));

            var procedureDtos = (ImportProcedureDto[])serializer.Deserialize(new StringReader(xmlString));

            var procedures = new List<Procedure>();

            foreach (var procDto in procedureDtos)
            {
                if (!IsValid(procDto) || !procDto.AnimalAids.All(IsValid))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var vet = context.Vets.FirstOrDefault(v => v.Name == procDto.VetName);
                if (vet == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var animal = context.Animals
                    .FirstOrDefault(a => a.PassportSerialNumber == procDto.AnimalSerialNumber);

                if (animal == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var procAnimalNames = procDto.AnimalAids.Select(f => f.Name).ToArray();
               var animalAidChecker = false;

                var allAnimalNames = context.AnimalAids.Select(a => a.Name).ToArray();

                foreach (var animalName in procAnimalNames)
                {
                    if (allAnimalNames.Contains(animalName) == false)
                    {
                        animalAidChecker = true;
                        break;
                    }
                }

                if (animalAidChecker == true)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }
                List<ProcedureAnimalAid> currentAnimalAids = new List<ProcedureAnimalAid>();
                bool duplicateAidChecker = false;
                foreach (var animalAidDto in procDto.AnimalAids)
                {
                    var animalAid = currentAnimalAids
                        .FirstOrDefault(a => a.AnimalAid.Name == animalAidDto.Name);

                    if (animalAid == null)
                    {
                        animalAid = new ProcedureAnimalAid()
                        {
                            AnimalAid = new AnimalAid()
                            {
                                Name = animalAidDto.Name
                                
                            }
                        };
                        currentAnimalAids.Add(animalAid);
                    }
                    else
                    {
                        duplicateAidChecker = true;
                        sb.AppendLine("Error: Invalid data.");
                        break;
                    }
                }
                if (duplicateAidChecker == false )
                {
                    sb.AppendLine("Record successfully imported.");

                    var procedure = new Procedure()
                    {
                        Animal = animal,
                        DateTime = DateTime
                        .ParseExact(procDto.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        ProcedureAnimalAids = currentAnimalAids,
                        Vet = vet
                    };

                    procedures.Add(procedure);
                }

            }

            context.Procedures.AddRange(procedures);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
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
