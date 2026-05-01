using data_access_layer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Modules.DTOs.Students;
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

    public Task<List<StudentWithStudentProfile>> GetAllStudentWithItProfile_UseLeftJoin()
    {
        var query =
            from s in dbContext.Students
            join sp in dbContext.StudentProfiles
                on s.StudentId equals sp.StudentId
                into ProfileGroup
            from p in ProfileGroup.DefaultIfEmpty()
            select new StudentWithStudentProfile
            {
                Student = s,
                Address = p.Address ?? "",
                City = p.City ?? "",
                Country = p.Country ?? "",
                Bio = p.Bio ?? "",
                LinkedInUrl = p.LinkedInUrl ?? ""
            };

        return query.ToListAsync();
    }

    public Task<List<StudentWithStudentProfile>> GetAllStudentWithItProfile_UseJoin()
    {
        return dbContext.Students
            .Join(
                dbContext.StudentProfiles,
                s => s.StudentId,
                sp => sp.StudentId,
                (student, studentProfile) => new StudentWithStudentProfile
                {
                    Student = student,
                    Address = studentProfile.Address ?? "",
                    City = studentProfile.City ?? "",
                    Country = studentProfile.Country ?? "",
                    Bio = studentProfile.Bio ?? "",
                    LinkedInUrl = studentProfile.LinkedInUrl ?? ""
                }
            )
            .ToListAsync();
    }

    public Task<List<StudentWithStudentProfile>> GetAllStudentWithItProfile_UseProjection()
    {
        return dbContext.Students
            .Select(s => new StudentWithStudentProfile
                {
                    Student = s,
                    Address = s.StudentProfile.Address ?? "",
                    City = s.StudentProfile.City ?? "",
                    Country = s.StudentProfile.Country ?? "",
                    Bio = s.StudentProfile.Bio ?? "",
                    LinkedInUrl = s.StudentProfile.LinkedInUrl ?? ""
                }
            )
            .ToListAsync();
    }

    public ValueTask<Student?> GetStudentById(int id)
    {
        return dbContext.Students.FindAsync(id);
    }
}
