using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class SubjectWorkProgramRepository : ISubjectWorkProgramRepository
{
    private UserContext Context { get; }
    
    public SubjectWorkProgramRepository(UserContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task AddAsync(SubjectWorkProgram entity)
    {
        var existingSubject = await Context.Subjects
            .FirstOrDefaultAsync(x => x.Id == entity.Subject.Id);
    
        if (existingSubject == null)
        {
            throw new Exception("Subject is not in database.");
        }
        
        entity.Subject = existingSubject;
    
        await Context.SubjectWorkPrograms.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<SubjectWorkProgram?> GetByIdAsync(Guid id)
    {
         return await Context.SubjectWorkPrograms
            .Include(x => x.Subject)
            .Include(x => x.Classes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<SubjectWorkProgram>> GetAllAsync()
    {
        return await Context.SubjectWorkPrograms
            .Include(x => x.Subject)
            .Include(x => x.Classes)
            .ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        Context.SubjectWorkPrograms.Remove(await GetByIdAsync(id));
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SubjectWorkProgram entity)
    {
        var existingEntity = await GetByIdAsync(entity.Id);

        if (existingEntity is not null)
        {
            var existingSubject = await Context.Subjects
                .FirstOrDefaultAsync(x => x.Id == entity.Subject.Id);
    
            if (existingSubject == null)
            {
                throw new Exception("Subject is not in database.");
            }
            
            existingEntity.Subject = existingSubject;
            existingEntity.Classes.Clear();
            
            foreach (var newClass in entity.Classes)
            {
                existingEntity.Classes.Add(newClass);
            }
            
            Context.Update(existingEntity);
            await Context.SaveChangesAsync();
        }
    }
}