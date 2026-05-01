using data_access_layer.Data;
using Microsoft.EntityFrameworkCore;
using Modules.Entities;

namespace data_access_layer.Repository;

public class StudentRepository(AppDbContext dbContext)
{
    public Task<List<Student>> GetAllStudents()
    {
        return dbContext.Students.ToListAsync();
    }

    public Task<List<Student>> GetAllStudentsByStatus(string status)
    {
        return dbContext.Students
            .Where(s => s.Status == status)
            .ToListAsync();
    }

    public ValueTask<Student?> GetStudentById(int id)
    {
        return dbContext.Students.FindAsync(id);
    }
}
