using AutoMapper;
using Daveslist.Domain.Users.Models;
using Daveslist.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authentication;

namespace Daveslist.Infrastructure.Identity.Mappers;

public class IdentityMapperProfile : Profile
{
    public IdentityMapperProfile()
    {
        CreateMap<RegisterModel, User>()
            .ForMember(destination => destination.CreatedDate, options => options.MapFrom(_ => DateTime.UtcNow));

        CreateMap<AuthenticationProperties, AuthenticationPropertiesModel>()
            .ForMember(destination => destination.Items, options => options.MapFrom(source => source.Items.ToDictionary(entry => entry.Key, entry => entry.Value)))
            .ForMember(destination => destination.Parameters, options => options.MapFrom(source => source.Parameters.Select(p => p.Key).ToList()));

        CreateMap<AuthenticationPropertiesModel, AuthenticationProperties>()
            .ForMember(destination => destination.Items, options => options.MapFrom(source => source.Items.ToDictionary(entry => entry.Key, entry => entry.Value)))
            .ForMember(destination => destination.Parameters, options => options.MapFrom(source => source.Parameters.Select(key => new KeyValuePair<string, object?>(key, null)).ToList()));

        CreateMap<User, UserModel>()
            .ForMember(destination => destination.Email, options => options.MapFrom(source => source.Email ?? string.Empty));

        CreateMap<UserModel, User>();
    }
}
