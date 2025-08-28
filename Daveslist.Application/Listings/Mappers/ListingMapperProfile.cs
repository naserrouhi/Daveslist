using AutoMapper;
using Daveslist.Application.Listings.Models;
using Daveslist.Domain.Listings.Models;


namespace Daveslist.Application.Listings.Mappers;

public class ListingMapperProfile : Profile
{
    public ListingMapperProfile()
    {
        CreateMap<UpsertListingDto, Listing>()
            .ConstructUsing((dto, context) =>
            {
                var userId = context.Items["UserId"] as int?;
                var pictures = dto.PictureUrls?.Select(u => new Picture(u)).ToList();

                return new(userId!.Value, dto.CategoryId, dto.Title!, dto.Content!, dto.IsPublic, pictures);
            });

        CreateMap<Listing, ListingDto>();
        CreateMap<Reply, ReplyDto>();
        CreateMap<Picture, PictureDto>();
    }
}
