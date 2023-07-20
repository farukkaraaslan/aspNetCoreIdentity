using AspNetCoreIdentity.Web.CustomValidations;
using AspNetCoreIdentity.Web.Extensions;
using AspNetCoreIdentity.Web.Models;
using AspNetCoreIdentity.Web.OptionsModel;
using AspNetCoreIdentity.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"))
    );
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddIdentityExtensions();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.ConfigureApplicationCookie(options =>
{
    var cookieBuilder = new CookieBuilder();
    cookieBuilder.Name = "IdentityCookie";
    //giriþ yapýlmasý gereken sayfaya eriþildiðinde yönlendirilecek sayfa
    options.LoginPath = new PathString("/Home/SingIn");
    //kullanýc çýkýþ yapacaðýnda yönlendirilecek sayfa
    options.LogoutPath = new PathString("/Member/Logout");
    options.Cookie=cookieBuilder;
    options.ExpireTimeSpan=TimeSpan.FromMinutes(30);
    //kullanýcý 30 dk boyunca 1 gitþ dahi yapsa cooki süresi 300 dk daha uzar
    options.SlidingExpiration = true;
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
//kimlik doðrulama middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
