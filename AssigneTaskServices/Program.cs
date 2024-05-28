using AssigneTaskServices;
using AssigneTaskServices.Data;
using AssigneTaskServices.Extension;
using AssigneTaskServices.Repository;
using AssigneTaskServices.Services;
using AssigneTaskServices.Services.Contract;
using AssigneTaskServices.Services.Implementation;
using AssigneTaskServices.Utility;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAssignTask, AssignTaskService>();
builder.Services.AddScoped<IUserTaskServices, UserTaskService>();
builder.Services.AddScoped(typeof(IRabbitMQRepository<>), typeof(RabbitMQRepository<>));
builder.Services.AddAuthentication();
builder.Services.AddHttpContextAccessor();
IMapper mapper = MappingConfig.RegisterConfig().CreateMapper();
builder.Services.AddSingleton(mapper);
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    //options.UseSqlServer(builder.Configuration.GetConnectionString("TaskAssignedConnectons"));
    options.UseSqlServer(connectionString);
});
builder.Services.AddHttpClient("GetTaskById", u => u.BaseAddress =
new Uri(builder.Configuration["ServiceUrls:TaskAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
builder.AddJwtAuthenticationServices();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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