using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class RegistrationRequestRepository : IRegistrationRequestRepository
{
    private UserContext Context { get; }
    
    public RegistrationRequestRepository(UserContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task CreateRegistrationRequest(RegistrationRequest registrationRequest)
    {
        await Context.RegistrationRequests.AddAsync(registrationRequest);
        await Context.SaveChangesAsync();
    }
    
    public async Task UpdateRegistrationRequest(Guid id, RegistrationRequest registrationRequest)
    {
        Context.RegistrationRequests.Update(registrationRequest);
        await Context.SaveChangesAsync();
    }

    public async Task<RegistrationRequest?> GetRegistrationRequestById(Guid registrationRequestId) => 
        await Context.RegistrationRequests.Where(x => x.Id == registrationRequestId)
                                            .Include(s => s.User)
                                            .FirstOrDefaultAsync();

    public async Task<List<RegistrationRequest>> GetPendingRegistrationRequests() => 
        await Context.RegistrationRequests.Where(x => x.Status == RegistrationRequestStatus.Pending)
                                            .Include(s => s.User)
                                            .ToListAsync();
}