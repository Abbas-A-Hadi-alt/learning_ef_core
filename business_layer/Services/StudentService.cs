using data_access_layer.Repository;
using Modules.DTOs.Students;
using Modules.Entities;

namespace business_layer.Services;

public class StudentService(StudentRepository repository)
{
    public async Task<List<Student>> GetAllStudents()
    {
        return await repository.GetAllStudents();
    }

    public async Task<List<Student>> GetAllStudentsByStatus(string status)
    {
        return await repository.GetAllStudentsByStatus(status);
    }

    public async Task<List<StudentWithStudentProfile>> GetAllStudentWithItProfile_UseLeftJoin()
    {
        return await repository.GetAllStudentWithItProfile_UseLeftJoin();
    }

    public async Task<List<StudentWithStudentProfile>> GetAllStudentWithItProfile_UseJoin()
    {
        return await repository.GetAllStudentWithItProfile_UseJoin();
    }

    public async Task<List<StudentWithStudentProfile>> GetAllStudentWithItProfile_UseProjection()
    {
        return await repository.GetAllStudentWithItProfile_UseProjection();
    }

    public async Task<Student?> GetStudentById(int id)
    {
        return await repository.GetStudentById(id);
    }
}
