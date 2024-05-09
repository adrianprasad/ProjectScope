    using ProjectScope.Entity;

public interface IStudent
{
    void Insert(Student registrationView);
    void Update(Student registrationView);
    void Delete(int Id);
    List<Student> GetAll();
    Student GetById(int id);
    Student Get(string Email, string Password);
    Student Get(string Email);
    void SaveCourseId(int courseId,int Id);
    void SaveChanges();
}
