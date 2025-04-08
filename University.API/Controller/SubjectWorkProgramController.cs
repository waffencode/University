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

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
}