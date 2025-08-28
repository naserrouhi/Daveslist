namespace Daveslist.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomCorsOrigin(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>()!;

        services.AddCors(options =>
        {
            options.AddPolicy("AllowedOrigins",
                builder =>
                {
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed(origin =>
                            allowedOrigins.Any(allowedOrigin => origin.StartsWith(allowedOrigin)));
                });
        });

        return services;
    }
}
