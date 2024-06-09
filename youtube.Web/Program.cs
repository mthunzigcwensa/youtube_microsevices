using Microsoft.AspNetCore.Authentication.Cookies;
using youtube.Web.Service;
using youtube.Web.Service.IService;
using youtube.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IVideoService, VideoService>();

SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
SD.VideoAPIBase = builder.Configuration["ServiceUrls:VideoAPI"];


builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IVideoService, VideoService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
var logger = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddConsole();
}).CreateLogger<Program>();

logger.LogInformation("Application starting up");

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
