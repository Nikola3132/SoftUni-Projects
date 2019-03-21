using AutoMapper;
using IT_Firm_DB.SynthesizedEntities;
using IT_Firm_DB_Data;
using IT_Firm_DB_Models.Entities;
using IT_Firm_DB_Models.Entities.OwnProps;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using EmployeesMapping.SynthesizedEntities;

namespace EmployeesMapping
{
    public  class Employees_Methods
    {
        public Employees_Methods(Mapper mapper,ref FirmDbContext context)
        {
            this.Mapper = mapper;
            this.Context = context;
        }
       public Mapper Mapper { get; set; }
        public FirmDbContext Context { get; set; }
        

        public void AddEmployee(string[] args)
        {
            string firstName = args[0];
            string lastName = args[1];
            decimal salary = decimal.Parse(args[2]);

            EmployeeDto emp = new EmployeeDto()
            {
                Name = new EmpName() { FirstName = firstName, LastName = lastName }
                ,
                Salary = salary
            };

            Employee newEmp = this.Mapper.CreateMappedObject<Employee>(emp);

           this.Context.Employees.Add(newEmp);
           this.Context.SaveChanges();
        }

        public void SetBirthday(string[] args)
        {
            int empId = int.Parse(args[0]);
            DateTime date = DateTime.ParseExact(args[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);

               var emp = this.Context.Employees.SingleOrDefault(e => e.EmployeeId == empId);

            if (emp == null)
            {
                throw new ArgumentException($"No such user with Id - {empId} in the database");
            }

            emp.BirthDay = date;

            var entry = this.Context.Entry(emp);
            entry.State = EntityState.Modified;

            this.Context.SaveChanges();

        }

        public void SetAddress(string[] args)
        {
            int empId = int.Parse(args[0]);
            string address = args[1];

            var emp = this.Context.Employees.SingleOrDefault(e => e.EmployeeId == empId);

            if (emp == null)
            {
                throw new ArgumentException($"No such user with Id - {empId} in the database");
            }

            emp.Address = address;

            this.Context.SaveChanges();
        }

        public void EmployeeInfo(string[] args)
        {
            int empId = int.Parse(args[0]);

            var emp = this.Context.Employees.SingleOrDefault(e => e.EmployeeId == empId);

            if (emp == null)
            {
                throw new ArgumentException($"No such user with Id - {empId} in the database");
            }

            var infoEmp = this.Mapper.CreateMappedObject<EmployeeDto>(emp);

            
            Console.WriteLine(infoEmp);
        }

        public void EmployeePersonalInfo(string[] args)
        {
            int empId = int.Parse(args[0]);
            
            var emp = this.Context.Employees.SingleOrDefault(e => e.EmployeeId == empId);

            if (emp == null)
            {
                throw new ArgumentException($"No such user with Id - {empId} in the database");
            }

            Console.WriteLine($"ID - {emp.EmployeeId} - {emp.FullName} " +
                $"- ${emp.Salary} \nBirthday: {emp.BirthDay?.ToString("d")}\n" +
                $"Address: {emp.Address}");
        }

        public void SetManager(string[] args)
        {
            int empId = int.Parse(args[0]);
            int managerId = int.Parse(args[1]);

            var firstEmp = this.Context.Employees.SingleOrDefault(e => e.EmployeeId == empId);
            var manager = this.Context.Employees.SingleOrDefault(e => e.EmployeeId == managerId);

            if (firstEmp == null || manager == null)
            {
                throw new ArgumentException("The employee or the manager do not exist");
            }

            firstEmp.Manager = manager;

            this.Context.Employees
                .SingleOrDefault(e => e.EmployeeId == managerId)
                .ManagedEmployees.Add(firstEmp);

            Context.SaveChanges();

        }

        public void ManagerInfo(string[] args)
        {
            int managerId = int.Parse(args[0]);

            var emp = this.Context.Employees.FirstOrDefault(m => m.EmployeeId == managerId);
            
           var manager = this.Mapper.CreateMappedObject<ManagerDto>(emp);

            Console.WriteLine(manager.ToString());
        }

        public void ListEmployeesOlderThan(string[] args)
        {
            int age = int.Parse(args[0]);

            if (!this.Context.Employees.Where(e => e.BirthDay != null).Any(e=>e.Manager != null))
            {
                //Seting Managers for some of the employees with birthday
                var EmpsForModify = this.Context.Employees.Where(e => e.BirthDay != null).Skip(2).ToList();

                List<string> commandForManagerInfo = new List<string>();
                int count = 1;
                foreach (var emp in EmpsForModify)
                {
                    commandForManagerInfo.Add(emp.EmployeeId.ToString() + " " + count.ToString());
                    count++;
                }
                foreach (var command in commandForManagerInfo)
                {
                    SetManager(command.Split(" ", StringSplitOptions.RemoveEmptyEntries));
                }
            }

            var emps = this.Context.Employees
                .Where(e => e.BirthDay != null && DateTime.Now.Year - e.BirthDay.Value.Year == age)
                .Select(e => new
                {
                    e.Name,
                    e.Salary,
                    e.ManagerId,
                    e.Manager
                })
                 .ToList() ;
            if (emps.Count() == 0)
            {
                Console.WriteLine("There are no users of this age");
                return;
            }
            foreach (var emp in emps)
            {
                string managerName = string.Empty;
                var manager = this.Context.Employees.FirstOrDefault(m => m.EmployeeId == emp.ManagerId);
                if (emp.Manager == null)
                {
                    managerName = "[no manager]";
                }
                else
                {
                    managerName =manager.Name.FirstName + " " + manager.Name.LastName;
                }
                Console.WriteLine($"{emp.Name.FirstName + " " + emp.Name.LastName} - ${emp.Salary:f2} - Manager: {managerName}");
            }
        }

        public void Exit()
        {
            Environment.Exit(0);
        }
    }
}
