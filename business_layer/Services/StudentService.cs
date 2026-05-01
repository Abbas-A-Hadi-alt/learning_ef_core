using data_access_layer.Repository;
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

    public async Task<Student?> GetStudentById(int id)
    {
        return await repository.GetStudentById(id);
    }
}
