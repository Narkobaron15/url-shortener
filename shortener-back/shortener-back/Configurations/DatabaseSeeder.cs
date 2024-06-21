namespace shortener_back.Configurations;

public static class DatabaseSeeder
{
    public static async Task SeedDatabase(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        
        IWebHostEnvironment env = services
            .GetRequiredService<IWebHostEnvironment>();
        ShortenerDbContext context = services
            .GetRequiredService<ShortenerDbContext>();

        await MigrateIfAnyIn(context);

        if (!env.IsDevelopment()) return;
        
        var userManager = services
            .GetRequiredService<UserManager<User>>();
        var roleManager = services
            .GetRequiredService<RoleManager<IdentityRole>>();
        IConfiguration configuration = services
            .GetRequiredService<IConfiguration>();

        await SeedRoles(roleManager);
        await SeedUsers(userManager, configuration);
    }

    private static async Task MigrateIfAnyIn(DbContext ctx)
    {
        if ((await ctx.Database.GetPendingMigrationsAsync()).Any())
            await ctx.Database.MigrateAsync();
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.FindByNameAsync("Admin") is null)
            await roleManager.CreateAsync(new IdentityRole("Admin"));

        if (await roleManager.FindByNameAsync("User") is null)
            await roleManager.CreateAsync(new IdentityRole("User"));
    }

    private static async Task SeedUsers(
        UserManager<User> userManager,
        IConfiguration configuration
    )
    {
        if (await userManager.FindByNameAsync(
                configuration["Admin:Username"]!
            ) is not null)
            return;

        User admin = new()
        {
            UserName = configuration["Admin:Username"],
            Email = configuration["Admin:Email"],
            EmailConfirmed = true,
            PhoneNumber = configuration["Admin:Phone"],
            PhoneNumberConfirmed = true
        };
        await userManager.CreateAsync(admin, configuration["Admin:Password"]!);
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}
