using AutoMapper;
using Daveslist.Application.Users.AppServices.Interfaces;
using Daveslist.Application.Users.Dtos;
using Daveslist.Domain.Shared.Exceptions;
using Daveslist.Domain.Users.Infrastructures;
using Daveslist.Domain.Users.Models;
using Microsoft.Extensions.Logging;

namespace Daveslist.Application.Users.AppServices;

public class UserAppService : IUserAppService
{
    private readonly IIdentityManager _identityManager;
    private readonly IMapper _mapper;
    private readonly ILogger<UserAppService> _logger;

    public UserAppService(IIdentityManager identityManager,
                                 IMapper mapper,
                                 ILogger<UserAppService> logger)
    {
        _identityManager = identityManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task RegisterAsync(RegisterDto registerDto)
    {
        _logger.LogInformation("Registering a new user with email: {Email}", registerDto.Email);

        var registerModel = _mapper.Map<RegisterModel>(registerDto);

        _logger.LogInformation("User registered successfully with email: {Email}", registerDto.Email);

        await _identityManager.RegisterAsync(registerModel);
    }

    public async Task LoginAsync(LoginDto loginDto)
    {
        _logger.LogInformation("User attempting to log in with username: {Username}", loginDto.Username);

        var loginModel = _mapper.Map<LoginModel>(loginDto);
        var isSuccess = await _identityManager.LoginAsync(loginModel);

        if (!isSuccess)
            throw new BusinessException("User login faced an error.");

        _logger.LogInformation("User logged in successfully with username: {Username}", loginDto.Username);
    }

    public AuthenticationPropertiesModel GetAuthenticationProperties(string provider, string? redirectUrl)
    {
        return _identityManager.GetAuthenticationProperties(provider, redirectUrl);
    }

    public async Task LogoutAsync()
    {
        await _identityManager.LogoutAsync();
    }
}
