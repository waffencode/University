using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class RegistrationRequestRepository : IRegistrationRequestRepository
{
    private RegistrationRequestContext Context { get; }
    
    public RegistrationRequestRepository(RegistrationRequestContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));
    
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

    public async Task<RegistrationRequest?> GetRegistrationRequestById(Guid registrationRequestId)
    {
        return await Context.RegistrationRequests.FindAsync(registrationRequestId);
    }
}