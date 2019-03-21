using IT_Firm_DB.SynthesizedEntities;
using IT_Firm_DB_Models.Entities;
using IT_Firm_DB_Models.Entities.OwnProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmployeesMapping.SynthesizedEntities
{
    public class ManagerDto
    {
        private ICollection<EmployeeDto> managedEmployees = new List<EmployeeDto>();

        public EmpName Name { get; set; }

        public ICollection<EmployeeDto> ManagedEmployees
        {
            get { return this.managedEmployees; }
            set { this.managedEmployees = value; }
        } 

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{Name.FirstName} {Name.LastName} | Employees: {this.ManagedEmployees.Count}");

            if (ManagedEmployees.Count == 0)
            {
                sb.AppendLine("This manager has not any employed workers");
            }
            else
            {
                foreach (var emp in ManagedEmployees)
                {
                    sb.AppendLine($"    - {Name.FirstName} {Name.LastName} - {emp.Salary}");
                }
            }
            return sb.ToString().Trim();
        }
    }
}
