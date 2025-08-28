using Daveslist.Domain.PrivateMessages.Models;
using Daveslist.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daveslist.Infrastructure.Data.Configs;

public class PrivateMessageConfiguration : IEntityTypeConfiguration<PrivateMessage>
{
    public void Configure(EntityTypeBuilder<PrivateMessage> builder)
    {
        builder.HasKey(pm => pm.Id);

        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        builder.Property(pm => pm.Content)
               .HasMaxLength(DomainRulesConstants.PrivateMessage.MaxContentLength);
    }
}
