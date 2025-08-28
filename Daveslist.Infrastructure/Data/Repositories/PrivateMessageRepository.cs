using Daveslist.Domain.PrivateMessages.Models;
using Daveslist.Domain.PrivateMessages.Repositories;

namespace Daveslist.Infrastructure.Data.Repositories;

public class PrivateMessageRepository : BaseRepository<PrivateMessage, int>, IPrivateMessageRepository
{
    public PrivateMessageRepository(DaveslistDbContext dbContext) : base(dbContext)
    {
    }
}
