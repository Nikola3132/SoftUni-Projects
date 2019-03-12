using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    [Table("StudentsCourses")]
    public class StudentCourse
    {
        public StudentCourse()
        {

        }

        public StudentCourse(int studentId, int courseId)
        {
            this.StudentId = studentId;
            this.CourseId = courseId;
        }

        [Required]
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}
