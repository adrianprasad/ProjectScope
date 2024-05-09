using ProjectScope.Entity;


namespace ProjectScope.Data
{
    public interface ICourse
    {
        void Insert(Course courseView);
        void Update(Course courseView);
        void Delete(int Id);
        List<Course> GetAll();
       
        Course GetById(int Id);
        void EnrollStudent(int courseId); 
        IEnumerable<Course> GetEnrolledCourses(int studentId); 
    }
}
