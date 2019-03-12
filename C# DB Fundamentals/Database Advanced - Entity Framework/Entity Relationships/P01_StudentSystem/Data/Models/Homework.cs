using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    [Table("HomeworkSubmissions")]
    public class Homework
    {
        public Homework()
        {

        }

        public Homework(int id,string content, ContentType contentType, int studentId, int courseId)
        {
            this.HomeworkId = id;

            this.Content = content;
            this.ContentType = contentType;

            this.StudentId = studentId;
            this.CourseId = courseId;

            this.SubmissionTime = DateTime.Now;
        }

        [Key]
        [Column("Id")]
        public int HomeworkId { get; set; }

        [Required]
        [FileExtensions(Extensions = ".application,.zip,.pdf", ErrorMessage = "It should be a file path")]
        [Column(TypeName = "VARCHAR(MAX)")]
        public string Content { get; set; }

        [Required]
        public ContentType ContentType { get; set; }

        [Required]
        public DateTime SubmissionTime { get; set; }

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


