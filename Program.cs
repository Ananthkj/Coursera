using Coursera.Data;
using Coursera.Models;
using Coursera.Services.Base;
using Coursera.Services.Email;
using Coursera.Services.Folder;
using Coursera.Services.Profile;
using Coursera.Services.SeedService;
using Coursera.Services.SignUp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<EmailServices>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//Bind the SmtpSettings section to the SmtpSettings class
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Configure Claims-Based Authentication using Cookies
/*builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Redirect to this path if unauthenticated
    options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect if access is denied
    options.ExpireTimeSpan = TimeSpan.FromDays(30); // Customize cookie expiration
    options.SlidingExpiration = true; // Renew cookies on each valid request
    options.Cookie.HttpOnly = true; // Protect cookies from JavaScript access
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure cookies are sent only over HTTPS
    options.Cookie.SameSite = SameSiteMode.Strict; // Mitigate CSRF attacks
});*/


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Configure Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireRole("Admin", "Instructor", "Student"); // User must have any one of these roles
    });
});

// Add Session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true; // Protect session cookies from JavaScript access
    options.Cookie.IsEssential = true; // Required for GDPR compliance
});


builder.Services.AddScoped<SeedService>();
builder.Services.AddScoped<IUsersService,UserService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IProfileService, ProfileService>();

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

app.UseSession();

using (var scope=app.Services.CreateScope())
{
  var seedService=  scope.ServiceProvider.GetRequiredService<SeedService>();
    seedService.Seed();
    //seedService.Seed2();
}

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
