using Daveslist.Domain.Categories.Models;
using Daveslist.Domain.Listings.Models;
using Daveslist.Domain.PrivateMessages.Models;
using Daveslist.Infrastructure.Data.Configs;
using Daveslist.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Daveslist.Infrastructure.Data;

public class DaveslistDbContext : IdentityDbContext<User, UserRole, int>
{
    public DaveslistDbContext(DbContextOptions<DaveslistDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Listing> Listings { get; set; }
    public DbSet<PrivateMessage> PrivateMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new ListingConfiguration());
        builder.ApplyConfiguration(new PrivateMessageConfiguration());
    }
}
