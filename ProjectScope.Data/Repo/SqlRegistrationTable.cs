using System.Collections.Generic;
using System.Linq;
using ProjectScope.Entity;

public class SqlRegistrationTable : IStudent
{
    private readonly EntityDbContext _context;

    public SqlRegistrationTable(EntityDbContext context)
    {
        _context = context;
    }

    public void Insert(Student registrationView)
    {

        _context.Students.Add(registrationView);
        _context.SaveChanges();
    }

    public void Update(Student registrationView)
    {
        _context.Students.Update(registrationView);
        _context.SaveChanges();
    }

    public void Delete(int Id)
    {
        var student = GetById(Id);
        if (student != null)
        {
            _context.Students.Remove(student);
            _context.SaveChanges();
        }
    }

    public List<Student> GetAll()  // Corrected method name
    {
        return _context.Students.ToList();
    }

    public Student GetById(int id)  
    {
        return _context.Students.Find(id);
    }

    public Student Get(string Email, string Password)
    {
        return _context.Students.FirstOrDefault(s => s.Email == Email && s.Password == Password);
    }

    public Student Get(string Email)
    {
        return _context.Students.FirstOrDefault(s => s.Email == Email);
    }

    public void SaveCourseId(int courseId, int studentId)
    {
        var student = _context.Students.FirstOrDefault(s => s.Id == studentId);
        if (student != null)
        {
            //if (!student.CourseId.Contains(courseId.ToString()))
            //{
                student.CourseId = student.CourseId + "," + courseId.ToString();
                _context.Students.Update(student);
                _context.SaveChanges();
            //}
        }
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
