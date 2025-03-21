using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;

namespace University.Controller;

[ApiController]
[Route("api/[controller]")]
public class SubjectWorkProgramController : ControllerBase
{
    private readonly SubjectWorkProgramRepository _repository;
    private ILogger<SubjectWorkProgramController> _logger;

    public SubjectWorkProgramController(UserContext context, ILogger<SubjectWorkProgramController> logger)
    {
        _repository = new SubjectWorkProgramRepository(context);
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Get(Guid id)
    {
        return Ok(await _repository.GetByIdAsync(id));
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _repository.GetAllAsync());
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<SubjectWorkProgram>> Post(SubjectWorkProgram program)
    {
        await _repository.AddAsync(program);
        var createdProgram = _repository.GetByIdAsync(program.Id);
        return CreatedAtAction(nameof(Get), routeValues: new { id = program.Id }, createdProgram);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Put(Guid id, SubjectWorkProgram program)
    {
        await _repository.UpdateAsync(program);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
}