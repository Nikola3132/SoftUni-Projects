using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    [Table("Courses")]
    public class Course
    {
        public Course()
        {

        }
        public Course(int id,string name, decimal price)
        {
            this.CourseId = id;

            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.Now.AddDays(60);

            this.Name = name;
            this.Price = price;
        }

        [Key]
        [Column("Id")]
        public int CourseId { get; set; }

        [Required]
        [StringLength(80)]
        [Column(TypeName = "NVARCHAR(80)")]
        public string Name { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "DATETIME2")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "DATETIME2")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(15,2)")]
        public decimal Price { get; set; }

        public ICollection<StudentCourse> StudentsEnrolled { get; set; } = new List<StudentCourse>();

        public ICollection<Resource> Resources { get; set; } = new List<Resource>();

        public ICollection<Homework> HomeworkSubmissions { get; set; } = new List<Homework>();
    }
}

