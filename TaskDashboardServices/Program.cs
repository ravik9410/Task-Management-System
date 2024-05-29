using TaskDashboardServices.Extensions;
using TaskDashboardServices.Services.Contract;
using TaskDashboardServices.Services.Implementation;
using TaskDashboardServices.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITaskDashboard, TaskDashboard>();
builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient("GetTask", u => u.BaseAddress =
new Uri(builder.Configuration["ServiceUrls:CreateTaskAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
builder.AddJwtAuthenticationServices();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Dashboard Services");
    c.InjectStylesheet("/swagger/custom.css");
    c.RoutePrefix = String.Empty;
});


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
