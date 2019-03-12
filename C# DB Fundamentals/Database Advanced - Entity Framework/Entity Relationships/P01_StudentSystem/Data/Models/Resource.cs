using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    [Table("Resources")]
    public class Resource
    {
        public Resource()
        {

        }

        public Resource(int id,string name, string url, ResourceType resourceType, int courseId)
        {
            this.ResourceId = id;

            this.Name = name;
            this.Url = url;
            this.ResourceType = resourceType;
            this.CourseId = courseId;
        }

        [Key]
        [Column("Id")]
        public int ResourceId { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "NVARCHAR(50)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(MAX)")]
        [Url]
        public string Url { get; set; }

        [Required]
        public ResourceType ResourceType { get; set; }

        [Required]
        public int CourseId { get; set; }


        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}

