using AspNetCoreIdentity.Web.CustomValidations;
using AspNetCoreIdentity.Web.localization;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.Extensions;

public static class StartupExtensions
{
    public static void AddIdentityExtensions(this IServiceCollection services)
    {
        services.Configure<DataProtectionTokenProviderOptions>(options =>
         {
             options.TokenLifespan = TimeSpan.FromMinutes(10);
         });
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            //email benzersi olmalı
            options.User.RequireUniqueEmail = true;
            //username için izin verilecek olan karakterler
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz1234567890_-";
            //gerken minimum şifre uzunlugu
            options.Password.RequiredLength = 6;
            //alfanumeric karakterler olmasın ?* gibi
            options.Password.RequireNonAlphanumeric = false;
            //küçük karakter zorunlu
            options.Password.RequireLowercase = true;
            // büyük karakter zorunlu değil
            options.Password.RequireUppercase = false;
            //sayısal karakter gerekliliği
            options.Password.RequireDigit = false;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            options.Lockout.MaxFailedAccessAttempts = 3;

        }).AddEntityFrameworkStores<AppDbContext>()
        .AddUserValidator<UserValidator>()
        .AddPasswordValidator<PasswordValidator>()
        .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
        // kullanıcı için identity default token üretirs
        .AddDefaultTokenProviders()
        ;
       

    }
}
