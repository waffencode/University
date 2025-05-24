using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Exceptions;
using University.Infrastructure;

namespace University.Repository;

public class MessageRepository : IMessageRepository
{
    private UniversityContext Context { get; }
    
    public MessageRepository(UniversityContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<List<Message>> GetMessagesByReceiver(User user)
    {
        return await Context.Messages.Where(m => m.Receivers.Contains(user) || m.Sender.Equals(user))
            .AsNoTracking()
            .AsSplitQuery()
            .Include(m => m.Sender)
            .Include(m => m.Receivers)
            .Include(m => m.ReceiversStudyGroups)
            .Include(m => m.RelatedClass)
            .ToListAsync();
    }
    
    public async Task<List<Message>> GetMessagesBySender(User user)
    {
        return await Context.Messages.Where(m => m.Sender.Equals(user))
            .Include(m => m.Sender)
            .Include(m => m.Receivers)
            .Include(m => m.ReceiversStudyGroups)
            .Include(m => m.RelatedClass)
            .ToListAsync();
    }

    public async Task AddAsync(MessageDto dto, CancellationToken cancellationToken = default)
    {
        var receivers = await Context.Users.Where(x => dto.ReceiversIds.Contains(x.Id)).ToListAsync(cancellationToken);
        var receiversStudyGroups = await Context.StudyGroups.Where(x => dto.ReceiversStudyGroupsIds.Contains(x.Id)).ToListAsync(cancellationToken);
        var relatedClass = await Context.ScheduleClasses.FirstOrDefaultAsync(x => x.Id == dto.RelatedClassId, cancellationToken);
        var sender = await Context.Users.FirstOrDefaultAsync(x => x.Id == dto.SenderId, cancellationToken);
        if (sender is null)
        {
            throw new EntityNotFoundException(typeof(User), dto.SenderId.ToString());
        }
        
        var entity = new Message
        {
            Id = dto.Id,
            Attachments = dto.Attachments,
            Date = dto.Date,
            IsImportant = dto.IsImportant,
            Receivers = receivers,
            ReceiversStudyGroups = receiversStudyGroups,
            RelatedClass = relatedClass,
            Sender = sender,
            Text = dto.Text,
            Topic = dto.Topic
        };
        
        await Context.Messages.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
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
        // TODO: Implement method
        throw new NotImplementedException();
    }
}