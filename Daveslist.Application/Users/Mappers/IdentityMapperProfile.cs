using AutoMapper;
using Daveslist.Application.Users.Dtos;
using Daveslist.Domain.Users.Models;

namespace Daveslist.Application.Users.Mappers;

public class IdentityMapperProfile : Profile
{
    public IdentityMapperProfile()
    {
        CreateMap<RegisterDto, RegisterModel>();

        CreateMap<LoginDto, LoginModel>();

        CreateMap<UserModel, UserDto>();
    }
}
