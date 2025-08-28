using Daveslist.Api.Extensions;
using Daveslist.Application.Shared.Extensions;
using Daveslist.Domain.Shared.Extensions;
using Daveslist.Infrastructure.Extensions;

namespace Daveslist.Api;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();
        services.AddControllers();
        services.AddDomainServices();
        services.AddDomainEvents();

        services.AddCustomIdentity();
        services.AddDbContext(configuration);
        services.AddInfrastructureMappers();
        services.AddRepositories();

        services.AddCustomCorsOrigin(configuration);
        services.AddExceptionHandlers();

        services.AddApplicationServices();
        services.AddApplicationMappers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.SeedRoles();

        app.UseCors("AllowedOrigins");

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseExceptionHandler();
    }
}
