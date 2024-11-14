using University.Domain;

namespace University.Repository;

public interface IRegistrationRequestRepository
{
    public Task CreateRegistrationRequest(RegistrationRequest registrationRequest);

    public Task UpdateRegistrationRequest(Guid id, RegistrationRequest registrationRequest);

    public Task<RegistrationRequest?> GetRegistrationRequestById(Guid registrationRequestId);
}