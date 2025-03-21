using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;

namespace University.Controller;

[Route("api/[controller]")]
[ApiController]
public class StudyGroupController : ControllerBase
{
    private readonly ILogger<StudyGroupController> _logger;
    private readonly StudyGroupRepository _studyGroupRepository;

    public StudyGroupController(UserContext context, ILogger<StudyGroupController> logger)
    {
        _logger = logger;
        _studyGroupRepository = new StudyGroupRepository(context);
    }

    [HttpGet]
    public async Task<ActionResult<List<StudyGroup>>> Get()
    {
        var studyGroups = await _studyGroupRepository.GetAllAsync();
        return Ok(studyGroups);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StudyGroup>> Get(Guid id)
    {
        return Ok(await _studyGroupRepository.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<StudyGroup>> Post(StudyGroup studyGroup)
    {
        await _studyGroupRepository.AddAsync(studyGroup);
        return CreatedAtAction(nameof(Get), new { id = studyGroup.Id }, studyGroup);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<StudyGroup>> Put(Guid id, StudyGroup studyGroup)
    {
        if (id != studyGroup.Id) return BadRequest();

        await _studyGroupRepository.UpdateAsync(studyGroup);
        return Ok(studyGroup);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<StudyGroup>> Delete(Guid id)
    {
        await _studyGroupRepository.DeleteAsync(id);
        return Ok();
    }
}