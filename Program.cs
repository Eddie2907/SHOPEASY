using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using dotnetapp.Managers;
using Microsoft.Win32.SafeHandles;
namespace dotnetapp
{
class Program
{
    static void Main(string[] args)
    {
 
    //     // Create instances of BLL  
        CourseManager courseManager = new CourseManager();
        InstructorManager instructorManager =new InstructorManager();
        int choice;
        do{
            Console.WriteLine("Course and Instructor Management Console App Menu:");
            Console.WriteLine("1. Add Instructor to list");
            Console.WriteLine("2.List Instructor from list");
            Console.WriteLine("3.Add Course to List");
            Console.WriteLine("4. List Courses from list");
            Console.WriteLine("5.Find Course from list");
            Console.WriteLine("6.Edit Course in list");
            Console.WriteLine("7.Delete Course from list");
            Console.WriteLine("8.Exit");
            Console.Write("Enter your choice:");
            choice=int.Parse(Console.ReadLine());
            switch(choice)
            {
                case 1:
                    instructorManager.AddInstructor();
                break;
                case 2:
                    courseManager.ListCourses();
                break;
                case 3:
                    Console.WriteLine("Enter Instructor ID for Course:");
                    int Id = int.Parse(Console.ReadLine());
                    courseManager.AddCourse(Id);
                    break;
                case 4:
                    instructorManager.ListInstructors();
                break;
                case 5:
                    Console.WriteLine("Enter Course Name to find: ");
                    string name = Console.ReadLine();
                    courseManager.FindCourse(name);
                break;
                case 6:
                    courseManager.EditCourse();
                break;
                case 7:
                courseManager.DeleteCourse();
                break;
                case 8:
                Console.WriteLine("Exiting from Course and Instructor Management Console App...");
                break;
                default:
                Console.WriteLine("Invalid Choice");
                break;
            }
        }
        while(choice!=0);
    }
}
}
 
program.cs
 


 
using System;
 
using System.Collections.Generic;
 
using System.Linq;
 
using System.Text;
 
using System.Threading.Tasks;
 
using System.Data;
 
using System.Data.SqlClient;
 
using System.Configuration;
 
using dotnetapp.Models;
 
using dotnetapp.Managers;
 
using dotnetapp.Service;
 
namespace dotnetapp.Managers
{
public class CourseManager:ICourseManager
 {
 public static List<Course> courses = new List<Course>();
 CoursesService courseService = new CoursesService();
    public void AddCourse(int courseId)
    {
        Course course = new Course();
        course.CourseId=CoursesService.Courses.Count+1;
        course.CourseId=courseId;
        Console.Write("Enter Instructor Id:");
        course.InstructorId = int.Parse(Console.ReadLine());
        Console.Write("Enter Course Title: ");
        course.Title=Console.ReadLine();
        Console.Write("Enter Course Description:");
        course.Description = (Console.ReadLine());
         Console.Write("Enter Course Duration:");
        course.Duration= int.Parse(Console.ReadLine());
        courseService.AddCourse(course);
        Console.WriteLine("Course added successfully!");
    }
    public void ListCourses()
    {
        if(CoursesService.Courses.Count==0)
        {
            Console.WriteLine("No Courses");
            return;
        }
        Console.WriteLine("List of Courses:");
        foreach(var course in CoursesService.Courses)
        {
            Console.WriteLine($"Course ID:{course.CourseId},Title:{course.Title},Description:{course.Description},Duration:{course.Duration},Instructor Id:${course.InstructorId}");
        }
    }
    public void FindCourse(string CourseName)
    {
        var course = CoursesService.Courses.FirstOrDefault(p=>p.Title.ToLower()==CourseName.ToLower());
        if(course == null)
            Console.WriteLine("Course not found!");
        else
            Console.WriteLine($"Course ID:{course.CourseId},Title:{course.Title},Description:{course.Description},Duration:{course.Duration},Instructor Id:${course.InstructorId}");
        }
        public Course FindById(int Id)
        {
            return CoursesService.Courses.FirstOrDefault(p=>p.CourseId==Id);
        }
        public void EditCourse()
        {
            Console.Write("Enter Course ID to edit:");
            int id = int.Parse(Console.ReadLine());
            var course = CoursesService.Courses.FirstOrDefault(p=>p.CourseId==id);
            if(course == null)
            {
                Console.WriteLine("Course not found!");
                return;
            }
            Console.Write("Enter new Course Name (leave empty to kepp current):");
            string name=Console.ReadLine();
            Console.Write("Enter new Course Description(leave empty to kepp current):");
            string description=Console.ReadLine();
            Console.Write("Enter new Course Duration (leave empty to kepp current):");
            int duration=int.Parse(Console.ReadLine());
            Console.Write("Enter new Bidding Price (leave empty to kepp current):");
            int instid=int.Parse(Console.ReadLine());
            if(!string.IsNullOrWhiteSpace(name))
                course.Title = name;
            if(!string.IsNullOrWhiteSpace(description))
                course.Description= description;
            Console.WriteLine("Course information updated successfully!.");
        }
        public void DeleteCourse()
        {
            Console.Write("Enter Course ID to delete:");
            int id = int.Parse(Console.ReadLine());
            var course = CoursesService.Courses.FirstOrDefault(p=>p.CourseId == id);
            if(course == null)
            {
                Console.WriteLine("Course not found!");
                return;
            }
            CoursesService.Courses.Remove(course);
            Console.WriteLine("Course deleted successfully!");
        }
        public void AddCourseToDB(int teamid)
        {
            courseService.AddCourseToDB(teamid);
            Console.WriteLine("Course added to database successfully!");
        }
         public void ListCoursesFromDB()
        {
 
            courseService.ListCoursesFromDB();
        }
        public void DeleteCourseFromDB(){
            courseService.DeleteCourseFromDB();
        }
        public void EditCourseInDB(){
            courseService.EditCourseInDB();
        }
}
}
 
 
course magaer
 
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnetapp.Models;
using dotnetapp.Service;
 
namespace dotnetapp.Managers
{
public class InstructorManager:IInstructorManager{
    InstructorService instructorService = new InstructorService();
    public static List<Instructor> instructors = new List<Instructor>();
    public void AddInstructor()
    {
        Instructor instructor = new Instructor();
        instructor.InstructorId = InstructorService.Instructors.Count+1;
        Console.WriteLine("Enter Instructor Name:");
        instructor.Name=Console.ReadLine();
        Console.WriteLine("Enter Email:");
        instructor.Email=(Console.ReadLine());
        Console.WriteLine("Enter HireDate");
         instructor.HireDate=DateTime.Parse(Console.ReadLine());
        instructorService.AddInstructor(instructor);
        Console.WriteLine("Instructor addded successfully!");
    }
    public void ListInstructors()
    {
        if(InstructorService.Instructors.Count == 0)
        {
            Console.WriteLine("No Instructors");
            return;
        }
        Console.WriteLine("List of Instructors:");
            foreach(var instructor in InstructorService.Instructors)
            {
                Console.WriteLine($"Instructor ID:{instructor.InstructorId},Name:{instructor.Name},Email:${instructor.Email},HireDate:${instructor.HireDate}");
            }
    }
    public void AddInstructorToDB(){
        Console.WriteLine("enter InstructorID: ");
        int id = int.Parse(Console.ReadLine());
        instructorService.AddInstructorsToDB(id);
    }
    public void ListInstructorsFromDB(){
        instructorService.ListInstructorsFromDB();
    }
}
}
 
instructor manager
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace dotnetapp.Managers
{
  public interface IInstructorManager{
    void AddInstructor();
    void ListInstructors();
    void AddInstructorToDB();
    void ListInstructorsFromDB();
  }  
}
 
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using dotnetapp.Models;
using System.Data.SqlClient;
namespace dotnetapp.Service
{
public class CoursesService
{
    private string connectionString = "User CourseId = sa; password = examlyMssql@123;Database = appdb;server= localhost";
    public static List<Course> Courses = new List<Course>();
    public void AddCourse(Course course)
    {
        Courses.Add(course);
    }
    public List<Course> ListCourses()
    {
        return Courses;
    }
    public Course FindCourse(string courseTitle)
    {
        return Courses.FirstOrDefault(p=>p.Title.Equals(courseTitle,StringComparison.OrdinalIgnoreCase));
    }
    public Course FindByCourseId(int courseId)
    {
        return Courses.FirstOrDefault(p=>p.CourseId==courseId);
    }
    public void EditCourse(Course updatedCourse)
    {
        Course existingCourse = Courses.FirstOrDefault(p=>p.CourseId == updatedCourse.CourseId);
        if(existingCourse != null)
        {
            existingCourse.Title = updatedCourse.Title;
            existingCourse.Description = updatedCourse.Description;
            existingCourse.Duration= updatedCourse.Duration;
            existingCourse.InstructorId = updatedCourse.InstructorId;
        }
    }
    public void DeleteCourse(int courseId)
    {
        Course course = Courses.FirstOrDefault(p=>p.CourseId == courseId);
        if(course!=null)
        {
            Courses.Remove(course);
        }
    }
    public void AddCourseToDB(int teamCourseId)
    {
        Console.WriteLine($"Enter Team CourseId for the Course: {teamCourseId}");
        Console.Write("Enter Course Title: ");
        string Title = Console.ReadLine();
        Console.Write($"Enter Course Age:");
        int age = int.Parse(Console.ReadLine());
        Console.Write("Enter Category: ");
        string category = Console.ReadLine();
        Console.Write("Enter BCourseIddingPrice: ");
        decimal bCourseIddingPrice = Convert.ToDecimal(Console.ReadLine());
        try
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "insert into Courses values(@Title,@Age,@Category,@BCourseIddingPrice,@TeamCourseId)";
            SqlCommand command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@Title",Title);
            command.Parameters.AddWithValue("@Age",age);
            command.Parameters.AddWithValue("@Category",category);
            command.Parameters.AddWithValue("@BCourseIddingPrice",bCourseIddingPrice);
            command.Parameters.AddWithValue("@TeamCourseId",teamCourseId);
            int rowsEffected = command.ExecuteNonQuery();
            if(rowsEffected>=1)
            {
                Console.WriteLine("Course added to the database successfully!");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public void ListCoursesFromDB()
    {
        SqlConnection connection = new SqlConnection(connectionString);
        string query = "select * from Courses";
        SqlCommand command = new SqlCommand(query,connection);
        SqlDataReader reader = command.ExecuteReader();
        while(reader.Read())
        {
            Console.WriteLine($"Course CourseId:{reader["CourseId"]}\nTitle:{reader["Title"]}\nAge:{reader["Age"]}\nCategory:{reader["Category"]}\nBCourseIdding Price:{reader["BCourseIddingPrice"]}\nTeam:{reader["TeamCourseId"]}");
        }
    }
    public void EditCourseInDB()
    {
        Console.Write("Enter Course CourseId to edit: ");
        int CourseId = int.Parse(Console.ReadLine());
        Console.Write("Enter new Course Title (leave empty to keep current): ");
        string Title = Console.ReadLine();
        Console.Write("Enter new Course Age (leave empty to keep current): ");
        int age = int.Parse(Console.ReadLine());
        Console.Write("Enter new Category (leave empty to keep current): ");
        string category = Console.ReadLine();
        Console.Write("Enter new BCourseIddingPrice (leave empty to keep current): ");
        decimal bCourseIddingPrice = decimal.Parse(Console.ReadLine());
        try
        {
            SqlConnection connection =new SqlConnection(connectionString);
            string query ="update Courses SET Title = @Title,age=@age,Category = @category,BCourseIddingPrice = @bCourseIddingprice,TeamCourseId =@teamCourseId where @CourseId=CourseId";
            SqlCommand command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@Title",Title);
            command.Parameters.AddWithValue("@Age",age);
            command.Parameters.AddWithValue("@Category",category);
            command.Parameters.AddWithValue("@BCourseIddingPrice",bCourseIddingPrice);
            int rowsEffected = command.ExecuteNonQuery();
            if(rowsEffected >= 1)
            {
                Console.WriteLine("Course Information updated successfully!");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public void DeleteCourseFromDB()
    {
        Console.Write("Enter Course CourseId to delete: ");
        int CourseId = int.Parse(Console.ReadLine());
        SqlConnection connection = new SqlConnection(connectionString);
        string query = "delete from Courses where CourseId=CourseId";
        SqlCommand command = new SqlCommand(query,connection);
        Console.WriteLine("Course deleted successfully!");
    }
}
}
 
 
course servive
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using dotnetapp.Models;
using dotnetapp.Managers;
using System.Data.SqlClient;
namespace dotnetapp.Service
{
public class InstructorService
{
    private string connectionString = "USer Id = sa; password = examlyMssql@123;Database = appdb;server= localhost";
    public static List<Instructor> Instructors = new List<Instructor>();
    public void AddInstructor(Instructor instructor)
    {
        Instructors.Add(instructor);
    }
    public List<Instructor> ListInstructors()
    {
        return Instructors;
    }
     public void AddInstructorsToDB(int instructorid)
    {
    Console.Write("Enter Instructor Name: ");
    string name = Console.ReadLine();
    Console.Write("Enter Email: ");
    string email = Console.ReadLine();
    Console.Write("Enter HireDate: ");
    DateTime hiredate = DateTime.Parse(Console.ReadLine());
       try{
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "insert into Instructors values(@Id,@Name,@Email,@HireDate)";
            SqlCommand command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@Id",instructorid);
            command.Parameters.AddWithValue("@Name",name);
            command.Parameters.AddWithValue("@Email",email);
            command.Parameters.AddWithValue("@HireDate",hiredate);
 
            int rowsEffected = command.ExecuteNonQuery();
            if(rowsEffected>=1)
            {
                Console.WriteLine("Instructor added to the database successfully!");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error:"+ex.Message);
        }
    }
    public void ListInstructorsFromDB()
    {
        SqlConnection connection = new SqlConnection(connectionString);
        string query = "select * from Instructors";
        SqlCommand command = new SqlCommand(query,connection);
        SqlDataReader reader = command.ExecuteReader();
        while(reader.Read())
        {
            Console.WriteLine($"Player ID:{reader["Id"]}\nName:{reader["Name"]}\n Email:{reader["Email"]}\n HireDate:{reader["HireDate"]}");
        }
   }
}
}
 
 
instrutor service
 
