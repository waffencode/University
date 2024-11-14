using Microsoft.EntityFrameworkCore;
using University.Infrastructure;
using University.Repository;
using University.Security;
using University.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.Configure<JwtTokenProvider>(builder.Configuration);

builder.Services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

// Custom security policy, defined in Security.ApiSecurityExtensions.
builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddApiAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors("default");
app.MapControllers();
app.Run();