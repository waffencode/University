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
        var contextSender = Context.Users.FirstOrDefault(u => u.Id == message.Sender.Id);
        
        if (contextSender is not null)
        {
            message.Sender = contextSender;
        }
        
        var receivers = message.Receivers.Select(receiver => Context.Users.FirstOrDefault(u => u.Id == receiver.Id)).OfType<User>().ToList();

        message.Receivers = receivers;
        
        await Context.Messages.AddAsync(message);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteMessage(Guid id)
    {
        var message = await Context.Messages.FirstOrDefaultAsync(m => m.Id == id) ??
                      throw new ArgumentException("Message not found");
        Context.Messages.Remove(message);
        await Context.SaveChangesAsync();
    }

    public Task UpdateMessage(Message message)
    {
        throw new NotImplementedException();
    }
}