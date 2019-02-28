using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class EmployeesInfo
    {
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
        
            foreach (var emp in context.Employees.OrderBy(e => e.EmployeeId))
            {
                string empInfo =
                    string.Join(" ", emp.FirstName, emp.LastName, emp.MiddleName, emp.JobTitle, $"{emp.Salary:f2}");

                sb.AppendLine(empInfo);
            }
            return sb.ToString();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var empInfos = context.Employees
                                    .Where(e => e.Salary > 50000)
                                    .OrderBy(e => e.FirstName)
                                    .Select(e => $"{e.FirstName} - {e.Salary:f2}")
                                    .ToList();

            return string.Join('\n', empInfos);
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => $"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:f2}")
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in employees)
            {
                sb.AppendLine(item);
            }

            return sb.ToString();

        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var adress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Employees.FirstOrDefault(e => e.LastName == "Nakov").Address = adress;

            context.SaveChanges(true);

            StringBuilder sb = new StringBuilder();
            foreach (var emp in context.Employees.Include(e => e.Address).OrderByDescending(e => e.AddressId).ToList().Take(10))
            {
                sb.AppendLine(emp.Address.AddressText);
            }

            return sb.ToString();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.EmployeesProjects
                .Where(e => e.Project.StartDate.Year >= 2001 && e.Project.StartDate.Year <= 2003)
                .Select(x => new
                {
                    EmployeeFullName = x.Employee.FirstName + " " + x.Employee.LastName,
                    ManagerFullName = x.Employee.Manager.FirstName + " " + x.Employee.Manager.LastName,
                    Projects = x.Employee.EmployeesProjects.Select(p => new
                    {
                        ProjectDesc = p.Project.Name,
                        StartDATE = p.Project.StartDate,
                        EndDATE = p.Project.EndDate
                    }).ToList()
                })
                .ToList()
                .Take(10);

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {


                sb.AppendLine($"{emp.EmployeeFullName} - Manager: {emp.ManagerFullName}");


                foreach (var p in emp.Projects)
                {
                    sb.AppendLine($"--{p.ProjectDesc} - {p.StartDATE.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {p.EndDATE?.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) ?? "not finished"}");
                }
            }

            return sb.ToString();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var adresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count())
                .ThenBy(t => t.Town.Name)
                .ThenBy(a => a.AddressText)
                .Select(a => new
                {
                    AdressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmpCount = a.Employees.Count()
                }).Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var ad in adresses)
            {
                sb.AppendLine($"{ad.AdressText}, {ad.TownName} - {ad.EmpCount} employees");
            }

            return sb.ToString();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var empProjects = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Projects = e.EmployeesProjects.Select(p => new
                    {
                        p.Project.Name
                    }).OrderBy(x => x.Name).ToList()
                }).FirstOrDefault();

            StringBuilder sb = new StringBuilder();

            if (empProjects == null)
            {
                throw new NullReferenceException("There is any user with ID -147");
            }

            sb.AppendLine($"{empProjects.FirstName} {empProjects.LastName} - {empProjects.JobTitle}");

            foreach (var p in empProjects.Projects)
            {
                sb.AppendLine(p.Name);
            }

            return sb.ToString();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(d => d.Employees.Count() > 5)
                .OrderBy(d => d.Employees.Count())
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepName = d.Name,
                    ManagerFullName = d.Manager.FirstName + " " + d.Manager.LastName,
                    EmpAndJobs = d.Employees.Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    }).OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName).ToList()
                })
                .ToList();

            StringBuilder sb = new StringBuilder();


            foreach (var d in departments)
            {
                sb.AppendLine($"{d.DepName} - {d.ManagerFullName}");

                foreach (var e in d.EmpAndJobs)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }
            return sb.ToString();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var projects = context.Projects
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .ToList();

            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine(p.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
            }

            return sb.ToString().TrimEnd();
        }

            public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering"
                                                    || e.Department.Name == "Tool Design"
                                                    || e.Department.Name == "Marketing"
                                                    || e.Department.Name == "Information Services")
                .ToList();

            for (int i = 0; i < employees.Count; i++)
            {
                employees[i].Salary += employees[i].Salary * 0.12m;
            }
            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
            }
            return sb.ToString();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var town = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            var addresses = town.Addresses.ToList();
            var addressesCount = addresses.Count();

            foreach (var address in addresses)
            {
                foreach (var employee in address.Employees)
                {
                    employee.AddressId = null;
                }
            }

            context.RemoveRange(addresses);
            context.Remove(town);
            context.SaveChanges();

            sb.AppendLine($"{addressesCount} addresses in Seattle were deleted");

            return sb.ToString().TrimEnd();
        }
    }
}
