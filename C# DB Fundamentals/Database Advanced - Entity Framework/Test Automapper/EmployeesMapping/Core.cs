using AutoMapper;
using IT_Firm_DB_Data;
using IT_Firm_DB_Models.Entities;
using IT_Firm_DB_Models.Entities.OwnProps;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EmployeesMapping
{
    public class Core
    {
        public Core(Mapper mapper, ref FirmDbContext context, Employees_Methods employees_Methods)
        {
            this.Mapper = mapper;
            this.Context = context;
            this.EmpMethods = employees_Methods;
        }
        public Mapper Mapper { get; set; }
        public FirmDbContext Context { get; set; }
        public Employees_Methods EmpMethods { get; set; }

        public void Work()
        {
            if (Context.Database.EnsureCreated())
            {
                Seed(Context);
            }
            Console.WriteLine("Please choose one and write one of theese commands ->" +
                              "\nAddEmployee <firstName> <lastName> <salary>" +
                              "\nSetBirthday <employeeId> <date: dd.MM.yyyy>" +
                              "\nSetAddress <employeeId> <address>" +
                              "\nEmployeeInfo <employeeId>" +
                              "\nEmployeePersonalInfo <employeeId>" +
                              "\nSetManager <employeeId> <managerId>" +
                              "\nManagerInfo <employeeId>" +
                              "\nListEmployeesOlderThan <age>" +
                              "\n \nIf you want to end the program -> Exit");
            try
            {

                while (true)
                {
                    string[] args = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim()).ToArray();

                    string command = args[0];

                    string[] values = args.Skip(1).ToArray();

                    Type type = typeof(Employees_Methods);

                    var method = type.GetMethod(command);

                    if (method == null)
                    {
                        Console.WriteLine("Your command does not exitst!");
                        continue;
                    }
                    if (values.Length == 0)
                    {
                        method.Invoke(this.EmpMethods, new object[0]);
                    }
                    else
                    {
                        method.Invoke(this.EmpMethods, new object[] { values });
                    }

                    Console.WriteLine("Your command was sucessfull.");
                }


            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong!");
            }
        }

        private static void Seed(FirmDbContext context)
        {
            Console.WriteLine("Please wait while the database get started");

            context.Database.EnsureDeleted();
            context.Database.Migrate();

            for (int i = 1; i <= 8; i++)
            {
                context.Employees.Add(new Employee()
                {
                    Name = new EmpName()
                    {
                        FirstName = $"FirstName_Test{i}",
                        LastName = $"LastName_Test{i}"
                    },
                    Salary = 249m + i * i * 3 * i
                });

            }

            //For The Last Task
            for (int i = 0; i < 5; i++)
            {
                context.Employees.Add(new Employee()
                {
                    Name = new EmpName()
                    {
                        FirstName = $"Last_Task_FirstName_Emp{i}",
                        LastName = $"Last_Task_LastName_Emp{i}"
                    },
                    Salary = 249m + i * i * 3 * i,
                    BirthDay = new DateTime(1989 + i, 1 + i, 1 + i * 2)
                });
            }
        }
    }

}
