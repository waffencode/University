using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Domain.Model;
using University.Exceptions;
using University.Repository;
using University.Service;

namespace University.Controller;

[ApiController]
[Route("api/[controller]")]
public class ScheduleClassController(IScheduleClassRepository repository, IScheduleClassService service, ILogger<ScheduleClassController> logger)
    : ControllerBase
{
    [Authorize(Policy = "RequireManagerRole")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateScheduleClass([FromBody] ScheduleClassDto scheduleClassDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Failed to create the schedule class {Id}: model state validation error occured.", scheduleClassDto.Id);
            return BadRequest("Failed to create the schedule class: model state validation error occured.");
        }

        try
        {
            await service.CreateScheduleClassAsync(scheduleClassDto, cancellationToken);
            return Created();
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("The user cancelled the create schedule class operation.");
            return StatusCode(499, "The operation was cancelled by the user.");
        }
        catch (EntityNotFoundException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError("An exception occurred when creating schedule class {Id}. Cause: {Message}", scheduleClassDto.Id, exception.Message);
            return StatusCode(500, exception.Message);
        }
    }

    [Authorize]
    [HttpGet]
    [ResponseCache(Duration = 30)]
    public async Task<IActionResult> GetScheduleClasses(CancellationToken cancellationToken)
    {
        return Ok(await repository.GetAllAsync(cancellationToken));
    }
    
    [Authorize]
    [HttpGet("{id:guid}")]
    [ResponseCache(Duration = 30)]
    public async Task<IActionResult> GetScheduleClassById(Guid id, CancellationToken cancellationToken)
    {
        var result = await repository.GetByIdAsync(id, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }

    [Authorize(Policy = "RequireTeacherRole")]
    [HttpPost("{id:guid}/journal")]
    public async Task<IActionResult> UpdateJournal(Guid id, [FromBody] ScheduleClassDetailsDto journalDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Failed to update the schedule class {Id} journal: model state validation error occured.", id);
            return BadRequest("Failed to update the schedule class journal: model state validation error occured.");
        }
        
        await service.UpdateScheduleClassJournalAsync(id, journalDto, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpGet("{id:guid}/groups")]
    public async Task<IActionResult> GetStudyGroupsForClass(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await service.GetStudyGroupsForClassAsync(id, cancellationToken));
        }
        catch (EntityNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError("An exception occurred when retrieving study groups for the schedule class {Id}. Cause: {Message}", id, exception.Message);
            return StatusCode(500, exception.Message);
        }
    }

    [Authorize]
    [HttpGet("{id:guid}/details")]
    public async Task<IActionResult> GetScheduleClassDetails(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await service.GetScheduleClassDetailsAsync(id, cancellationToken));
        }
        catch (Exception exception)
        {
            logger.LogError("An exception occurred when retrieving the schedule class {Id} details. Cause: {Message}", id, exception.Message);
            return StatusCode(500, exception.Message);
        }
    }
}