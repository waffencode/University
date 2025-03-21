using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;

namespace University.Controller;

[ApiController]
[Route("api/[controller]")]
public abstract class CRUDBaseController<T>: ControllerBase where T : class
{
    private readonly ILogger _logger;
    protected IBaseRepository<T> _repository;
    
    protected CRUDBaseController(UserContext context, ILogger<ClassTimeSlotController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<T>>> GetAll()
    {
        var classTimeSlots = await _repository.GetAllAsync();
        return Ok(classTimeSlots);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<T>> GetById(Guid id)
    {
        var classTimeSlot = await _repository.GetByIdAsync(id);
        if (classTimeSlot is null)
        {
            return NotFound();
        }
        
        return Ok(classTimeSlot);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<T>> Add(T entity)
    {
        await _repository.AddAsync(entity);
        return Ok(entity);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public abstract Task<ActionResult<T>> Update(Guid id, T entity);

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
}