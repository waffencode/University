using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Exceptions;
using University.Infrastructure;
using University.Repository;

namespace University.Controller;

[ApiController]
[Route("api/[controller]")]
public class SubjectWorkProgramController : ControllerBase
{
    private readonly SubjectWorkProgramRepository _repository;
    private ILogger<SubjectWorkProgramController> _logger;

    public SubjectWorkProgramController(UniversityContext context, ILoggerFactory loggerFactory)
    {
        _repository = new SubjectWorkProgramRepository(context, loggerFactory.CreateLogger<SubjectWorkProgramRepository>());
        _logger = loggerFactory.CreateLogger<SubjectWorkProgramController>();
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Get(Guid id)
    {
        var result = await _repository.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetAll()
    {
        var result = await _repository.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<SubjectWorkProgram>> Post(SubjectWorkProgram program)
    {
        await _repository.AddAsync(program);
        var createdProgram = await _repository.GetByIdAsync(program.Id);
        return CreatedAtAction(nameof(Get), routeValues: new { id = program.Id }, createdProgram);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Put(Guid id, SubjectWorkProgram program)
    {
        await _repository.UpdateAsync(program);
        return Ok();
    }

    /// <summary>
    /// Asynchronously deletes a <see cref="SubjectWorkProgram"/> entity with the specified ID.
    /// </summary>
    /// <param name="id">The GUID of the <see cref="SubjectWorkProgram"/> entity to delete.</param>
    /// <param name="cancellationToken">Cancellation token to cancel deletion.</param>
    /// <response code="200">The <see cref="SubjectWorkProgram"/> was successfully deleted.</response>
    /// <response code="404">Entity with the specified ID not found in database.</response>
    /// <response code="503">The operation was canceled or the service is unavailable.</response>
    /// <response code="400">The request is invalid.</response>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.DeleteAsync(id, cancellationToken);
            return Ok();
        }
        catch (EntityNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
        catch (OperationCanceledException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, exception.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}