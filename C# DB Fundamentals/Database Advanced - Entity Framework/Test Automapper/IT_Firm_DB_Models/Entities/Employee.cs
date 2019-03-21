using IT_Firm_DB_Models.Entities.OwnProps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IT_Firm_DB_Models.Entities
{
   [Table("Employees")]
    public class Employee
    {
        [Key]
        [Column("Id")]
        public int EmployeeId { get; set; }

        [Required]
        public EmpName Name { get; set; }

        [Required]
        [MinLength(200)]
        public decimal Salary { get; set; }

        public DateTime? BirthDay { get; set; }
        
        public string Address { get; set; }

        
        public int? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public Employee Manager { get; set; }

        [NotMapped]
        public ICollection<Employee> ManagedEmployees { get; set; } 
            = new List<Employee>();

        [NotMapped]
        public string FullName => this.Name.FirstName + " " + this.Name.LastName;
    }
}
