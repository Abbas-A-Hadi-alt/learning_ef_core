using data_access_layer.Repository;
using Modules.DTOs.Course;

namespace business_layer.Services;

public sealed class CourseService(CourseRepository courseRepository)
{
    public async Task<List<CourseWithInstructorFullNameDto>> GetEachCourseWithInstructorFullName()
    {
        return await courseRepository.GetEachCourseWithInstructorFullName();
    }
}
