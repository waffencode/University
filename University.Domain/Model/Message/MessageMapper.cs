namespace University.Domain;

public class MessageMapper
{
    public static MessageDto MessageToMessageDto(Message message)
    {
        return new MessageDto
        {
            Id = message.Id,
            Attachments = message.Attachments,
            Date = message.Date,
            IsImportant = message.IsImportant,
            ReceiversIds = message.Receivers.Select(x => x.Id).ToList(),
            ReceiversStudyGroupsIds = message.ReceiversStudyGroups.Select(x => x.Id).ToList(),
            RelatedClassId = message.RelatedClass?.Id,
            SenderId = message.Sender.Id,
            Text = message.Text,
            Topic = message.Topic
        };
    }
}