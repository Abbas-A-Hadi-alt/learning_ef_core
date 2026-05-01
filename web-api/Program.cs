using System.Security.Claims;
using Azure.Identity;
using web_api;

var builder = WebApplication.CreateBuilder(args);

// Remove the Server header for security hardening
builder.WebHost.UseKestrel(options => options.AddServerHeader = false);


var keyVaultUrl = builder.Configuration["KeyVault:Url"];
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUrl), 
        new DefaultAzureCredential());
}


builder.Services
    // I add all web.api layer related services in this method, you can check it in DependencyInjection.cs file
    .AddPresentation(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();
app.Use(async(context, next) =>
{
    await next();

    if (context.Response.StatusCode is StatusCodes.Status429TooManyRequests)
    {
        await context.Response.WriteAsync("Too many loging attemots. Try again later.");
    }
});

app.UseCors("EntityFrameworkCoreWithAPIsSecurity_APICorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

// Global 403 (Forbidden) logging middleware.
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode is StatusCodes.Status403Forbidden)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var path = context.Request.Path.ToString();

        app.Logger.LogWarning(
            "Forbidden access, UserId = '{UserId}', IP='{IP}', Path='{Path}'", 
            userId, 
            ip, 
            path);
    }
});

app.MapControllers();

app.Run();
