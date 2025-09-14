using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using University.Domain;

namespace University.Security;

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> to add authentication and authorization.
/// </summary>
public static class ApiSecurityExtensions
{
    /// <summary>
    /// Adds authentication and authorization to specified services collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> to add authentication and authorization on.</param>
    /// <param name="configuration"><see cref="IConfiguration"/> to get JwtOptions from.</param>
    public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        if (jwtOptions is not null)
        {
            // Using JWT for authentication.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    };
                    
                    // Define OnMessageReceived event during JWT validation.
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Pass the token from cookie to the message context.
                            context.Token = context.Request.Cookies["token"];
                            // Continue processing.
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

    /// <summary>
    /// Defines authorization policies for specified services collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> to define policies on.</param>
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