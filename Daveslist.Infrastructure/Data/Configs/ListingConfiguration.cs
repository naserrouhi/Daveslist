using Daveslist.Domain.Listings.Models;
using Daveslist.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daveslist.Infrastructure.Data.Configs;

public class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        builder.Property(c => c.Title)
               .HasMaxLength(DomainRulesConstants.Listing.MaxTitleLength);

        builder.Property(c => c.Content)
               .HasMaxLength(DomainRulesConstants.Listing.MaxContentLength);

        builder.OwnsMany(l => l.Pictures, p =>
        {
            p.ToTable($"{nameof(Listing)}{nameof(Listing.Pictures)}");
            p.WithOwner().HasForeignKey("ListingId");
            p.Property<int>("Id");
            p.HasKey("Id");
            p.Property(pi => pi.Url).HasMaxLength(DomainRulesConstants.Listing.MaxPictureUrlLength);
        });

        builder.OwnsMany(l => l.Replies, r =>
        {
            r.ToTable($"{nameof(Listing.Replies)}");
            r.WithOwner().HasForeignKey(rp => rp.ListingId);
            r.HasKey(rp => rp.Id);
            r.Property(rp => rp.Content).HasMaxLength(DomainRulesConstants.Listing.MaxReplyLength);
        });
    }
}
