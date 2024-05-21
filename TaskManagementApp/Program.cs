
using Microsoft.AspNetCore.Authentication.Cookies;
using TaskManagementApp.Service.Contract;
using TaskManagementApp.Service.Implementation;
using TaskManagementApp.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAuthentication, AuthenticationService>();
builder.Services.AddScoped<IBaseServices, BaseService>();
builder.Services.AddScoped<ITokenHandler, TokenHandler>();
builder.Services.AddScoped<IManageUser, UserManageServices>();
builder.Services.AddScoped<ITaskManagement, TaskManagement>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
StaticData.AuthUrl = builder.Configuration["APIUrls:AuthUrl"];
StaticData.TaskDashboardUrl = builder.Configuration["APIUrls:APIGateWayUrl"];
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.LogoutPath = "/Auth/Logout";
    options.LoginPath = "/Auth/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(1);

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
