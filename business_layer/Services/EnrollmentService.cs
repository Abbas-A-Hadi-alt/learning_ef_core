using data_access_layer.Repository;
using Modules.DTOs.Students;

namespace business_layer.Services;

public class EnrollmentService(EnrollmentRepository repository)
{
    public async Task<List<StudentCourseCountDto>> GetEachStudentWithNumberOfRegisteredCourses()
    {
        return await repository.GetEachStudentWithNumberOfRegisteredCourses();
    }
}
