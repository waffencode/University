using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;

namespace University.Controller;

public class FieldOfStudyController: CrudBaseController<FieldOfStudy>
{
    public FieldOfStudyController(UniversityContext context, ILogger<FieldOfStudyController> logger) : base(context, logger)
    {
        _repository = new FieldOfStudyRepository(context);
    }
    
    public override async Task<ActionResult<FieldOfStudy>> Update(Guid id, FieldOfStudy entity)
    {
        await _repository.UpdateAsync(entity);
        return Ok();
    }
}