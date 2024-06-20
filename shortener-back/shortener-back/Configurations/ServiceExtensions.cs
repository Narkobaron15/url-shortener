namespace shortener_back.Configurations;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services
    ) => services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

    public static IServiceCollection AddBusinessLogic(
        this IServiceCollection services
    )
    {
        services.AddScoped<IShortenService, ShortenServiceImpl>();
        // more services may be here

        return services;
    }

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

    public static IServiceCollection AddIdentity(
        this IServiceCollection services
    )
    {
        services.AddIdentity<User, IdentityRole>(
                options => options.SignIn.RequireConfirmedAccount = false
            )
            .AddEntityFrameworkStores<ShortenerDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }

}
