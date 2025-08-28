using AutoMapper;
using Daveslist.Application.PrivateMessages.Models;
using Daveslist.Domain.PrivateMessages.Models;
using Daveslist.Domain.PrivateMessages.Repositories;
using Daveslist.Domain.Users.Infrastructures;

namespace Daveslist.Application.PrivateMessages.AppServices;

public class PrivateMessageAppService : IPrivateMessageAppService
{
    private readonly IPrivateMessageRepository _privateMessageRepository;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;
    private readonly IIdentityManager _identityManager;

    public PrivateMessageAppService(IPrivateMessageRepository privateMessageRepository,
                                    IUserContext userContext,
                                    IMapper mapper,
                                    IIdentityManager identityManager)
    {
        _privateMessageRepository = privateMessageRepository;
        _userContext = userContext;
        _mapper = mapper;
        _identityManager = identityManager;
    }

    public async Task SendAsync(int toUserId, string content, CancellationToken cancellationToken)
    {
        var toUser = await _identityManager.FindByIdAsync(toUserId);
        if (toUser is null)
            throw new KeyNotFoundException("User not found.");

        var fromUserId = _userContext.GetCurrentUserId().GetValueOrDefault();
        var privateMessage = new PrivateMessage(fromUserId, toUserId, content);

        await _privateMessageRepository.AddAsync(privateMessage, cancellationToken);
        await _privateMessageRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<PrivateMessageDto>> GetListAsync(CancellationToken cancellationToken)
    {
        var userId = _userContext.GetCurrentUserId().GetValueOrDefault();

        var privateMessages = await _privateMessageRepository.GetListAsync(m => m.FromUserId == userId || m.ToUserId == userId, cancellationToken);

        return _mapper.Map<IEnumerable<PrivateMessageDto>>(privateMessages);
    }
}
