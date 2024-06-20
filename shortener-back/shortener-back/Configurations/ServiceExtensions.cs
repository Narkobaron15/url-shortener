namespace shortener_back.Configurations;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
    
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        return services.AddScoped<IShortenService, ShortenService>();
    }
}
