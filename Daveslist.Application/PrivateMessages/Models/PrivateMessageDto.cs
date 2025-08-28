namespace Daveslist.Application.PrivateMessages.Models;

public record PrivateMessageDto(int FromUserId, int ToUserId, string Content, DateTime SentAt);
