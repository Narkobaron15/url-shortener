WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddDbContext();

builder.Services.AddAutoMapper(typeof(MapperConfigs));

builder.Services.AddRepositories();

builder.Services.AddIdentity();
builder.Services.AddAuth(builder.Configuration);

builder.Services.AddBusinessLogic();

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();

app.UseHttpsRedirection();

string? origin = builder.Environment.IsDevelopment()
    ? builder.Configuration["MainPage"]
    : Environment.GetEnvironmentVariable("MainPage");
if (origin is null)
    throw new ApplicationException("MainPage is not set");

Console.WriteLine($"MainPage: {origin}");

app.UseCors(opts =>
    opts.WithOrigins(origin)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.SeedDatabase();

await app.RunAsync();
