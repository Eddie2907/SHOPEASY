
using Microsoft.EntityFrameworkCore;
namespace dotnetapp.Models;

public class AppDbContext : DbContext
{
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {}
   
       
       public DbSet <Student> Students {get; set;}
       public DbSet <Course> Courses {get; set;}

       protected override void OnModelCreating(ModelBuilder m)
       {

        m.Entity<Student>()
        .HasOne( s=> s.Course)
        .WithMany( c => c.Students)
        .HasForeignKey( s => s.CourseId);

        m.Entity<Course>().HasData
        (
                new Course { CourseId = 1, Title = "Mathematics 101", Description = "Basic Mathematics"},

                 new Course { CourseId = 2, Title = "History 101", Description = "Introduction to History"}

        );
       }
    
}

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace dotnetapp.Models
{
    // Write your Course class here...
    public class Course
    {
        public int CourseId {get; set;}

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title {get; set;}

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description {get; set;}

        public ICollection<Student> Students {get; set;}

    }
}


using System.ComponentModel.DataAnnotations;
namespace dotnetapp.Models
{

    // Write your Student class here...
    public class Student{

         public int StudentId {get; set;}

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name {get; set;}

    [Required]
    [EmailAddress]
    public string Email {get; set;}

    public int YearOfJoining {get; set;}

    public int CourseId {get; set;}

    public Course Course {get; set;}

    }
   
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;

using System.Threading.Tasks;

namespace dotnetapp.Controllers
{
    // [Route("[controller]")]
   public class StudentController : Controller
   {
    private readonly AppDbContext db;

    public StudentController(AppDbContext context)
    {
        db = context;

    }

    // public async Task<IActionResult> DisplayAllStudents()
    // {
    //     var students = await db.Students.Include(s => s.Course).ListAsync();
    //     return View(students);
    // }

    public IActionResult DisplayAllStudents()
    {
        var students = db.Students.Include(s => s.Course).ToList();
        return View(students);
    }

    public IActionResult SearchStudentsByName()
    {
        return View();
    }

    public async Task<IActionResult> SearchStudentsByName(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return RedirectToAction(nameof(DisplayAllStudents));
        }

        var students = await db.Students.Include(s => s.Course)
        .Where( s => s.Name.ToLower().Contains(query.ToLower())).ToListAsync();
        return View("DisplayAllStudents",students);
    }

    public ActionResult AddStudent()
    {
        ViewBag.Courses = new SelectList(db.Courses, "CourseId", "Title");
        return View();
    }

    [HttpPost]

    public IActionResult AddStudent(Student student)
    {
        db.Add(student);
        db.SaveChanges();
        return RedirectToAction(nameof(DisplayAllStudents));

        ViewBag.Courses = new SelectList(db.Courses,"CourseId","Title");
        return View(student);
    }

    public IActionResult UpdateStudent(int id)
    {
        var student = db.Students.Find(id);
        if(student == null)
        {
            return NotFound();
        }

        ViewBag.Courses = new SelectList(db.Courses, "CourseId", "Title");
        return View(student); 
    }

    [HttpPost]
    public IActionResult UpdateStudent(Student updatedStudent)
    {

        var studentToUpdate = db.Students.Find(updatedStudent.StudentId);
        studentToUpdate.Name = updatedStudent.Name;
         studentToUpdate.Email = updatedStudent.Email;
          studentToUpdate.YearOfJoining = updatedStudent.YearOfJoining;
           studentToUpdate.CourseId = updatedStudent.CourseId;
           db.SaveChanges();
           return RedirectToAction(nameof(DisplayAllStudents));

           ViewBag.Courses = new SelectList(db.Courses,"CourseId","Title");
           return View(updatedStudent);
    }
   }

}


