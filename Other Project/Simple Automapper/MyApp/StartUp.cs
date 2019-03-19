using AutoMapper;
using MyApp.Models;
using MyApp.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyApp
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration();

            MapperConfiguration mapConfig = mapperConfiguration.CreateMap();
            Mapper mapper = mapConfig.Mapper;

            List<Pet> pets = new List<Pet>() {
                new Pet() {Age = 2,Name = "Lucky", PetType = PetEnum.Cat },
                new Pet() {Age = 1,Name = "Cesar", PetType = PetEnum.Dog },
                new Pet() {Age = 5,Name = "Spartak", PetType = PetEnum.Reptile }
            };

            Person person = new Person()
            {
                FirstName = "Stefan",
                LastName = "Ivanov",
                Pets = pets
            };


            Student student = mapper.CreateMappedObject<Student>(person);

            Console.WriteLine("The person :\n" + JsonConvert.SerializeObject(person) + "\n \n");

            Console.WriteLine("The student :\n" + JsonConvert.SerializeObject(student));
        }
    }
}
