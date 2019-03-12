using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext(DbContextOptions options) : base(options)
        {
        }

        public StudentSystemContext()
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<StudentCourse>().HasKey(k => new { k.StudentId, k.CourseId });
            
            builder.Entity<Course>().HasMany(cs => cs.StudentsEnrolled).WithOne(c => c.Course);
            builder.Entity<Course>().HasMany(r => r.Resources).WithOne(s => s.Course);
            builder.Entity<Course>().HasMany(h => h.HomeworkSubmissions).WithOne(h => h.Course);

            builder.Entity<Homework>().HasOne(c => c.Course).WithMany(c => c.HomeworkSubmissions);

            builder.Entity<Resource>().HasOne(r => r.Course).WithMany(h => h.Resources);

            builder.Entity<Student>().HasMany(c => c.CourseEnrollments).WithOne(s=>s.Student);
            builder.Entity<Student>().HasMany(s => s.HomeworkSubmissions).WithOne(s => s.Student);
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.SqlConnectionStr);
        }
    }
}

