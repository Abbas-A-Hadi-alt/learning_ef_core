using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace web_api.Authorization;

public sealed class StudentOwnerOrAdminHandler
    : AuthorizationHandler<StudentOwnerOrAdminRequirement, int>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        StudentOwnerOrAdminRequirement requirement, 
        int studentId)
    {
        // Admin Override.
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Ownership Check.
        string? userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (int.TryParse(userId, out int authoizedStudentId) && authoizedStudentId == studentId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
