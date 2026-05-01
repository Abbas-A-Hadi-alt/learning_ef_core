using data_access_layer.Data;
using Microsoft.EntityFrameworkCore;
using Modules.DTOs.Course;

namespace data_access_layer.Repository;

public sealed class CourseRepository(AppDbContext dbContext)
{
    public Task<List<CourseWithInstructorFullNameDto>> GetEachCourseWithInstructorFullName()
    {
        return dbContext.Courses
            .Select(c => new CourseWithInstructorFullNameDto
            {
                Code = c.Code,
                Title = c.Title,
                InstructorFirstName = c.Instructor.FirstName,
                InstructorLastName = c.Instructor.LastName
            })
            .OrderBy(c => c.Title)
            .ToListAsync();
    }

}
