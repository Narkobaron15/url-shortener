namespace shortener_back.Configurations;

public static class ApplicationConfigs
{
    public static IApplicationBuilder ConfigureCors(
        this IApplicationBuilder app,
        IHostApplicationBuilder builder
    )
    {
        string? origin = builder.Environment.IsDevelopment()
            ? builder.Configuration["MainPage"]
            : Environment.GetEnvironmentVariable("MainPage");
        if (origin is null)
            throw new ApplicationException("MainPage is not set");

        app.UseCors(opts =>
            opts.WithOrigins(origin)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
        );
        
        return app;
    }
}
