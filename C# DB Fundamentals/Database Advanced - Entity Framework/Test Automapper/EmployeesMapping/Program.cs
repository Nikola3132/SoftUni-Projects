using AutoMapper;
using IT_Firm_DB.Initializer;
using IT_Firm_DB_Data;
using IT_Firm_DB_Models.Entities;
using IT_Firm_DB_Models.Entities.OwnProps;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EmployeesMapping
{
    class Program
    {
        static void Main(string[] args)
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration();

            var mapper = mapperConfiguration.CreateMap().Mapper;

            var core = new Core<FirmDbContext>("IT_Firm_DB_Data");
            var context = (FirmDbContext)core.Context;
            
            var empMethods = new Employees_Methods(mapper, ref context);

            Core coreEngine = new Core(mapper, ref context, empMethods);
            
            coreEngine.Work();
        }
    }
}
