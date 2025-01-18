using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class MessageRepository : IMessageRepository
{
    private UserContext Context { get; }
    
    public MessageRepository(UserContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<List<Message>> GetMessagesByReceiver(User user)
    {
        return await Context.Messages.Where(m => m.Receivers.Contains(user))
            .Include(m => m.Sender)
            .Include(m => m.Receivers)
            .ToListAsync();
    }

    public async Task<List<Message>> GetMessagesBySender(User user)
    {
        return await Context.Messages.Where(m => m.Sender.Equals(user))
            .Include(m => m.Sender)
            .Include(m => m.Receivers)
            .ToListAsync();
    }

    public async Task AddMessage(Message message)
    {
        await Context.Messages.AddAsync(message);
        await Context.SaveChangesAsync();
    }

    public Task DeleteMessage(Message message)
    {
        throw new NotImplementedException();
    }

    public Task UpdateMessage(Message message)
    {
        throw new NotImplementedException();
    }
}