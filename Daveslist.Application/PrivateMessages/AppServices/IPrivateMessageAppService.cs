using Daveslist.Application.PrivateMessages.Models;

namespace Daveslist.Application.PrivateMessages.AppServices;

public interface IPrivateMessageAppService
{
    Task<IEnumerable<PrivateMessageDto>> GetListAsync(CancellationToken cancellationToken);
    Task SendAsync(int toUserId, string content, CancellationToken cancellationToken);
}
