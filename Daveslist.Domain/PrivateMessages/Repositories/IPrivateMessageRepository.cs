using Daveslist.Domain.PrivateMessages.Models;
using Daveslist.Domain.Shared.Interfaces.Repositories;

namespace Daveslist.Domain.PrivateMessages.Repositories;

public interface IPrivateMessageRepository : IBaseRepository<PrivateMessage, int>
{
}
