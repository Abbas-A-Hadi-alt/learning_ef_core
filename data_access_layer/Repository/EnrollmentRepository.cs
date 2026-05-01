using data_access_layer.Data;
using Microsoft.EntityFrameworkCore;
using Modules.DTOs.Students;

namespace data_access_layer.Repository;

public class EnrollmentRepository(AppDbContext dbContext)
{
    /*
     * My SQL query:
        SELECT 
	        s.StudentId, 
	        (s.FirstName + ' ' + s.LastName) AS FullName,
	        COUNT(*) AS NumberOfRegisteredCourses
        FROM Enrollments AS e
	        INNER JOIN Students AS s
		        ON s.StudentId = e.StudentId
        GROUP BY s.StudentId, (s.FirstName + ' ' + s.LastName)
        ORDER BY NumberOfRegisteredCourses DESC, FullName
     */

    public Task<List<StudentCourseCountDto>> GetEachStudentWithNumberOfRegisteredCourses()
    {
        // Chat GPT: (1) based on my SQL query
        /*
        Executed DbCommand (25ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
              SELECT [t].[StudentId], [t].[FullName], COUNT(*) AS [NumberOfRegisteredCourses]
              FROM (
                  SELECT [e].[StudentId], [s].[FirstName] + N' ' + [s].[LastName] AS [FullName]
                  FROM [Enrollments] AS [e]
                  INNER JOIN [Students] AS [s] ON [e].[StudentId] = [s].[StudentId]
              ) AS [t]
              GROUP BY [t].[StudentId], [t].[FullName]
        */
        /*
        return dbContext.Enrollments
            .GroupBy(e => new
            {
                StudentId = e.StudentId,
                FullName = e.Student.FirstName + " " + e.Student.LastName
            })
            .Select(g => new StudentCourseCountDto 
                {
                    StudentId = g.Key.StudentId,
                    FullName = g.Key.FullName,
                    NumberOfRegisteredCourses = g.Count()
                }
            ).ToListAsync();//*/



        //////////////////////
        // Chat GPT: (2) based on my SQL query
        /*
        Executed DbCommand (47ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
              SELECT [t].[StudentId0] AS [StudentId], [t].[FullName], COUNT(*) AS [NumberOfRegisteredCourses]
              FROM (
                  SELECT [s].[StudentId] AS [StudentId0], [s].[FirstName] + N' ' + [s].[LastName] AS [FullName]
                  FROM [Enrollments] AS [e]
                  INNER JOIN [Students] AS [s] ON [e].[StudentId] = [s].[StudentId]
              ) AS [t]
              GROUP BY [t].[StudentId0], [t].[FullName]
         */
        /*
        return dbContext.Enrollments
            .Join(dbContext.Students,
                e => e.StudentId,
                s => s.StudentId,
                (e, s) => new
                {
                    s.StudentId,
                    FullName = s.FirstName + " " + s.LastName
                })
            .GroupBy(x => new { x.StudentId, x.FullName })
            .Select(g => new StudentCourseCountDto
            {
                StudentId = g.Key.StudentId,
                FullName = g.Key.FullName,
                NumberOfRegisteredCourses = g.Count()
            })
            .ToListAsync();//*/



        //////////////////////
        // Dr. Muhammad Abu-Hadhoud (1)
        /*
        Executed DbCommand (42ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
              SELECT [s].[StudentId], [s].[FirstName] + N' ' + [s].[LastName] AS [FullName], (
                  SELECT COUNT(*)
                  FROM [Enrollments] AS [e]
                  WHERE [s].[StudentId] = [e].[StudentId]) AS [NumberOfRegisteredCourses]
              FROM [Students] AS [s]
        */
        /*
        return dbContext.Students
            .Include(s => s.Enrollments)
            .Select(s => new StudentCourseCountDto
            {
                StudentId = s.StudentId,
                FullName = s.FirstName + " " + s.LastName,
                NumberOfRegisteredCourses = s.Enrollments.Count()
            })
            .ToListAsync();//*/


        //////////////////////
        // Dr. Muhammad Abu-Hadhoud (2)
        /*
        Executed DbCommand (53ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
              SELECT [s].[StudentId], [s].[FirstName] + N' ' + [s].[LastName] AS [FullName], (
                  SELECT COUNT(*)
                  FROM [Enrollments] AS [e]
                  WHERE [s].[StudentId] = [e].[StudentId]) AS [NumberOfRegisteredCourses]
              FROM [Students] AS [s]
        */
        return dbContext.Students
            .Select(s => new StudentCourseCountDto
            {
                StudentId = s.StudentId,
                StudentFirstName = s.FirstName,
                StudentLastName = s.LastName,
                NumberOfRegisteredCourses = s.Enrollments.Count()
            })
            .ToListAsync();
    }
}
