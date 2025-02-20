using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;

namespace University.Controller;

[Route("api/[controller]")]
[ApiController]
public class ClassroomController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly EfRepository<Classroom> _classroomRepository;
    
    public ClassroomController(UserContext userContext, ILogger<UserController> logger)
    {
        _logger = logger;
    }
}