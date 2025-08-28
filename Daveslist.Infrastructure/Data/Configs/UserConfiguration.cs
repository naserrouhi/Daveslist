using Daveslist.Domain.Shared.Constants;
using Daveslist.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daveslist.Infrastructure.Data.Configs;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName)
               .HasMaxLength(DomainRulesConstants.User.MaxFirstNameLength);

        builder.Property(u => u.LastName)
               .HasMaxLength(DomainRulesConstants.User.MaxFirstNameLength);
    }
}
