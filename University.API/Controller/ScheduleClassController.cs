using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Exceptions;
using University.Repository;

namespace University.Controller;

[ApiController]
[Route("api/[controller]")]
public class ScheduleClassController(IScheduleClassRepository repository, ILogger<ClassTimeSlotController> logger)
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
            await repository.AddAsync(scheduleClassDto, cancellationToken);
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
}