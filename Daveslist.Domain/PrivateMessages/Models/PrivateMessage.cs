using Daveslist.Domain.Shared.Constants;
using Daveslist.Domain.Shared.Exceptions;
using Daveslist.Domain.Shared.Models;

namespace Daveslist.Domain.PrivateMessages.Models;

public class PrivateMessage : AggregateRoot<int>
{
    protected PrivateMessage() { }

    public PrivateMessage(int fromUserId, int toUserId, string content)
    {
        if (string.IsNullOrWhiteSpace(content) || content.Length > DomainRulesConstants.PrivateMessage.MaxContentLength)
            throw new BusinessException($"Private message must be 1-{DomainRulesConstants.PrivateMessage.MaxContentLength} characters.");

        FromUserId = fromUserId;
        ToUserId = toUserId;
        Content = content;
        SentAt = DateTime.UtcNow;
    }

    public int FromUserId { get; private set; }
    public int ToUserId { get; private set; }
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
}
