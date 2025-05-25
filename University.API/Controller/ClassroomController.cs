using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;

namespace University.Controller;

[Route("api/[controller]")]
[ApiController]
public class ClassroomController(IClassroomRepository repository, ILogger<ClassroomController> logger) : ControllerBase
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClassrooms()
    {
        return Ok(await repository.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClassroom(Guid id)
    {
        var classroom = await repository.GetByIdAsync(id);
        if (classroom == null)
        {
            return NotFound();
        }

        return Ok(classroom);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateClassroom(Classroom classroom)
    {
        await repository.AddAsync(classroom);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateClassroom(Guid id, Classroom classroom)
    {
        var classroomToUpdate = await repository.GetByIdAsync(id);
        if (classroomToUpdate == null)
        {
            return NotFound();
        }

        await repository.UpdateAsync(classroom);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteClassroom(Guid id)
    {
        await repository.DeleteAsync(id);
        return Ok();
    }
}