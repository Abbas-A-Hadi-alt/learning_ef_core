using Microsoft.AspNetCore.Authorization;

namespace web_api.Authorization;

public sealed class StudentOwnerOrAdminRequirement 
    : IAuthorizationRequirement {}
