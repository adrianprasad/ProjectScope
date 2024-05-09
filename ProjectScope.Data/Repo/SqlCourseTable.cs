using Microsoft.EntityFrameworkCore;
using ProjectScope.Entity;

namespace ProjectScope.Data.Repo
{
    public class SqlCourseTable : ICourse
    {
        private readonly EntityDbContext _dbContext;
        private Course course;
        public SqlCourseTable(EntityDbContext entityDbContext)
        {
            _dbContext = entityDbContext;
        }
        public void Delete(int Id)
        {
            Course course = _dbContext.Course.FirstOrDefault(a => a.Id == Id);
            if (course != null)
            {
                _dbContext.Course.Remove(course);
                _dbContext.SaveChanges();
            }
        }

        public List<Course> GetAll()
        {
            return _dbContext.Course.ToList();
        }

        public Course GetById(int Id)
        {
            return _dbContext.Course.FirstOrDefault(a=>a.Id==Id);
        }

       

        public void Insert(Course courseView)
        {
            _dbContext.Course.Add(courseView);
            _dbContext.SaveChanges();
        }

        public void Update(Course courseView)
        {
            _dbContext.Course.Update(courseView);
            _dbContext.SaveChanges();
        }

        public void EnrollStudent(int courseId) // Student ID parameter removed
        {

            // Create a new enrollment entity (assuming Enrollment has CourseId and StudentId)
            var enrollment = new Course { Id = courseId};

            // Add the enrollment to the DbContext
            _dbContext.Course.Add(enrollment);

            // Save changes to persist the enrollment
            _dbContext.SaveChanges();
        }

        public IEnumerable<Course> GetEnrolledCourses(int studentId)
        {
            throw new NotImplementedException();
        }
    }
}
