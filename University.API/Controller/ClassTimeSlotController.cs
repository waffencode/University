using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;

namespace University.Controller;

[ApiController]
[Route("api/[controller]")]
public class ClassTimeSlotController : ControllerBase
{
    private readonly ILogger<ClassTimeSlotController> _logger;
    private readonly ClassTimeSlotRepository _repository;
    
    public ClassTimeSlotController(UserContext context, ILogger<ClassTimeSlotController> logger)
    {
        _logger = logger;
        _repository = new ClassTimeSlotRepository(context);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<ClassTimeSlot>>> GetAll()
    {
        var classTimeSlots = await _repository.GetAllAsync();
        return Ok(classTimeSlots);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<ClassTimeSlot>> GetById(Guid id)
    {
        var classTimeSlot = await _repository.GetByIdAsync(id);
        if (classTimeSlot == null) return NotFound();
        return Ok(classTimeSlot);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ClassTimeSlot>> Add(ClassTimeSlot classTimeSlot)
    {
        await _repository.AddAsync(classTimeSlot);
        return Ok(classTimeSlot);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<ClassTimeSlot>> Update(Guid id, ClassTimeSlot classTimeSlot)
    {
        if (id != classTimeSlot.Id)
        {
            return BadRequest();
        }
        
        await _repository.UpdateAsync(classTimeSlot);
        return Ok(classTimeSlot);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
}