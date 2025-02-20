using Microsoft.AspNetCore.Authorization;
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
    private readonly ClassroomRepository _classroomRepository;
    
    public ClassroomController(UserContext userContext, ILogger<UserController> logger)
    {
        _logger = logger;
        _classroomRepository = new ClassroomRepository(userContext);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClassrooms()
    {
        return Ok(await _classroomRepository.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClassroom(Guid id)
    {
        var classroom = await _classroomRepository.GetByIdAsync(id);
        if (classroom == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(classroom);
        }
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateClassroom(Classroom classroom)
    {
        await _classroomRepository.AddAsync(classroom);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateClassroom(Guid id, Classroom classroom)
    {
        var classroomToUpdate = await _classroomRepository.GetByIdAsync(id);
        if (classroomToUpdate == null)
        {
            return NotFound();
        }

        await _classroomRepository.UpdateAsync(classroom);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteClassroom(Guid id)
    {
        await _classroomRepository.DeleteAsync(id);
        return Ok();
    }
}