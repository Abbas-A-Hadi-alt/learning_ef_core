using business_layer.Services;
using data_access_layer.Data;
using data_access_layer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;
using web_api.Authorization;

namespace web_api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddRateLimiterInternal()
            .AddAuthenticationInternal(configuration)
            .AddAuthorizationInternal()
            .AddEntityFrameworkCore(configuration)
            .AddServicesInternal()
            .AddControllersInternal() // If use minimal APIs, you must cahnge it with your own method to add minimal APIs
            .AddSwaggerInternal()
            .AddCorsInternal();

    private static IServiceCollection AddServicesInternal(this IServiceCollection services)
    {
        services.AddScoped<StudentService>();
        services.AddScoped<StudentRepository>();

        services.AddScoped<EnrollmentService>();
        services.AddScoped<EnrollmentRepository>();

        services.AddScoped<CourseService>();
        services.AddScoped<CourseRepository>();

        return services;
    }

    private static IServiceCollection AddEntityFrameworkCore(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new KeyNotFoundException("'DefaultConnection' key is not found in Connection Strings.");
        
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }

    private static IServiceCollection AddRateLimiterInternal(this IServiceCollection services)
    {
        services.AddRateLimiter(options => 
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy("AuthLimiter", httpContext =>
            {
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ip, 
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0
                    });
            });
        });
        
        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "EFCoreWithAPIsSecurity",
                ValidAudience = "EFCoreWithAPIsSecurityUsers",
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]
                        ?? throw new KeyNotFoundException("'JWT_SECRET_KEY' key is not found in Enviroment Variables")))
            };
        });

        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("StudentOwnerOrAdmin",
                policy => policy.Requirements.Add(new StudentOwnerOrAdminRequirement()));
        });

        services.AddScoped<IAuthorizationHandler, StudentOwnerOrAdminHandler>();

        return services;
    }


    private static IServiceCollection AddControllersInternal(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }

    private static IServiceCollection AddSwaggerInternal(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authentication header using the Bearer scheme.",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        }
                    },
                    []
                }
            });
        });

        return services;
    }

    private static IServiceCollection AddCorsInternal(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("EntityFrameworkCoreWithAPIsSecurity_APICorsPolicy", policy =>
            {
                policy.WithOrigins(
                        "https://localhost:7010",
                        "http://localhost:5208"
                        )
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        return services;
    }
}
