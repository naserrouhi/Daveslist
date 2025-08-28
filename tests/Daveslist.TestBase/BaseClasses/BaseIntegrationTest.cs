using Daveslist.Application.Shared.Extensions;
using Daveslist.Domain.Shared.Extensions;
using Daveslist.Infrastructure.Data;
using Daveslist.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Daveslist.TestBase.BaseClasses;

public abstract class BaseIntegrationTest : IAsyncDisposable
{
    protected readonly ServiceProvider _serviceProvider;
    protected readonly DaveslistDbContext _dbContext;

    protected BaseIntegrationTest()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder().Build();

        services.AddDomainServices();
        services.AddDomainEvents();

        services.AddCustomIdentity();
        services.AddInfrastructureMappers();
        services.AddRepositories();

        services.AddApplicationServices();
        services.AddApplicationMappers();

        services.AddDbContext<DaveslistDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        _serviceProvider = services.BuildServiceProvider();
        _dbContext = _serviceProvider.GetRequiredService<DaveslistDbContext>();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
    }
}
