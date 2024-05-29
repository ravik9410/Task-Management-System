using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskCreationService;
using TaskCreationService.Data;
using TaskCreationService.Extensions;
using TaskCreationService.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
IMapper mapper = MappingConfig.RegisterConfig().CreateMapper();
builder.Services.AddSingleton(mapper);
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";
builder.Services.AddDbContext<AppDbContext>(options =>
{
    //options.UseSqlServer(builder.Configuration.GetConnectionString("TaskCreateConnectons"));
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();
builder.AddJwtAuthenticationServices();
builder.Services.AddHttpClient("GetUser", u => u.BaseAddress =
new Uri(builder.Configuration["ServiceUrls:UserAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Create Services");
    c.InjectStylesheet("/swagger/custom.css");
    c.RoutePrefix = String.Empty;
});


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();
void ApplyMigration()
{

    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}