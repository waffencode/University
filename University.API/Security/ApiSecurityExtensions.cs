using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using University.Domain;

namespace University.Security;

public static class ApiSecurityExtensions
{
    public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        if (jwtOptions is not null)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience  = false,
                        ValidateLifetime  = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["token"];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }
        else
        {
            throw new NullReferenceException("JWTOptions is null");
        }
    }

    public static void AddApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", policy => 
                policy.RequireRole(nameof(UserRole.Admin)))
            .AddPolicy("RequireManagerRole", policy => 
                policy.RequireAssertion(context =>
                    context.User.IsInRole(nameof(UserRole.Admin)) ||
                    context.User.IsInRole(nameof(UserRole.Manager))))
            .AddPolicy("RequireTeacherRole", policy => 
                policy.RequireAssertion(context =>
                    context.User.IsInRole(nameof(UserRole.Admin)) ||
                    context.User.IsInRole(nameof(UserRole.Manager)) ||
                    context.User.IsInRole(nameof(UserRole.Teacher))))
            .AddPolicy("RequireStudentRole", policy => 
                policy.RequireAssertion(context =>
                    context.User.IsInRole(nameof(UserRole.Admin)) ||
                    context.User.IsInRole(nameof(UserRole.Manager)) ||
                    context.User.IsInRole(nameof(UserRole.Teacher)) ||
                    context.User.IsInRole(nameof(UserRole.Student))));
    }
}