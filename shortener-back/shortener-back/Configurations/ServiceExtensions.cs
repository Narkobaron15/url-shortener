using shortener_back.Services;

namespace shortener_back.Configurations;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services
    ) => services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

    public static IServiceCollection AddBusinessLogic(
        this IServiceCollection services
    ) => services.AddScoped<IShortenService, ShortenServiceImpl>();

    public static IHostApplicationBuilder AddDbContext(
        this IHostApplicationBuilder builder
    )
    {
        builder.Services.AddDbContext<ShortenerDbContext>(
            options => options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection")
            )
        );
        return builder;
    }
}
