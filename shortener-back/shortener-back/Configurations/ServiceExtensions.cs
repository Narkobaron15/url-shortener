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
        services.AddScoped<ITokensRepository, TokensRepositoryImpl>();
        services.AddScoped<IUserService, UserServiceImpl>();
        services.AddScoped<IJwtTokenService, JwtTokenServiceImpl>();

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

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // swagger ui configs
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "You api title", Version = "v1" });
            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme 
                { 
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT", 
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
    
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
        
        // auth configs
        byte[] keyBytes = Encoding.UTF8.GetBytes(configuration["JwtSecretKey"]!);
        SymmetricSecurityKey signingKey = new(keyBytes);
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = signingKey,
                ValidateAudience = false, // on production make true
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtIssuer"],
                ClockSkew = TimeSpan.Zero
            };
            cfg.Events = new JwtBearerEvents {
                OnAuthenticationFailed = context => {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers["IS-TOKEN-EXPIRED"] = "true";
                    }
                    return Task.CompletedTask;
                },
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["jwt"];
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}
