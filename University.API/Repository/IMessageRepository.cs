using University.Domain;

namespace University.Repository;

public interface IMessageRepository
{
    public Task<List<Message>> GetMessagesByReceiver(User user);

    public Task<List<Message>> GetMessagesBySender(User user);
    
    public Task AddMessage(Message message);
    
    public Task DeleteMessage(Guid id);
    
    public Task UpdateMessage(Message message);
}