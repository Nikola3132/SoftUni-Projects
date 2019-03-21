using IT_Firm_DB_Models.Entities.OwnProps;
using System;
using System.Collections.Generic;
using System.Text;

namespace IT_Firm_DB.SynthesizedEntities
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }

        public EmpName Name { get; set; }
       
        public decimal Salary { get; set; }

        public override string ToString()
        {
            return $"{EmployeeId} - {Name.FirstName} {Name.LastName} - ${Salary:f2}";
            //   return $"EmpId - {EmployeeId} \nFirstName - {Name.FirstName} \nLastName - {Name.LastName} \nSalary - ${Salary:f2}";
        }
    }
}
