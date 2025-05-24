using System.Reflection;
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
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(origin => new Uri(origin).IsLoopback);
        policy.WithOrigins("http://localhost:5432")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
});

builder.Services.AddDbContext<UniversityContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRegistrationRequestRepository, RegistrationRequestRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStudyGroupRepository, StudyGroupRepository>();
builder.Services.AddScoped<IClassTimeSlotRepository, ClassTimeSlotRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IScheduleClassRepository, ScheduleClassRepository>();
builder.Services.AddScoped<ISubjectWorkProgramRepository, SubjectWorkProgramRepository>();
builder.Services.AddScoped<IScheduleClassService, ScheduleClassService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.Configure<JwtTokenProvider>(builder.Configuration);
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

// TODO: Find way to store timestamp in database without using this.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("default");
app.Run();