using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    [Table("Students")]
    public class Student
    {
        public Student()
        {

        }

        public Student(int id,string name, DateTime registeredOn, DateTime? birthday)
        {
            this.StudentId = id;

            this.Name = name;
            this.RegisteredOn = registeredOn;
            this.Birthday = birthday;
        }

        [Key]
        [Column("Id")]
        public int StudentId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The name shouldn't up more than 100 charactes")]
        [Column(TypeName = "NVARCHAR(100)")]
        public string Name { get; set; }

        [StringLength(10, ErrorMessage = "The phone should be exactly 10 charactes", MinimumLength = 10)]
        [Column(TypeName = "CHAR(10)")]
        public string PhoneNumber { get; set; }

        [Required]
        [Column(TypeName = "DATETIME2")]
        public DateTime RegisteredOn { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime? Birthday { get; set; }


        public ICollection<StudentCourse> CourseEnrollments { get; set; } = new List<StudentCourse>();

        public ICollection<Homework> HomeworkSubmissions { get; set; } = new List<Homework>();
    }
}

