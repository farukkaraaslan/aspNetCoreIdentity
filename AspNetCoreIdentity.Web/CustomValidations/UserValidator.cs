using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspNetCoreIdentity.Web.CustomValidations;

public class UserValidator : IUserValidator<AppUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
    {
        var errors = new List<IdentityError>();
        var isDigit = int.TryParse(user.UserName[0].ToString(), out _);

        if (isDigit)
        {
            errors.Add(new IdentityError
            {
                Code = "UsernameContainFirstLetterDigit",
                Description = "Kullanıcı adı sayısal karakterle başlayamaz."
            });
        }

        if (errors.Any())
        {
            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }

        return Task.FromResult(IdentityResult.Success);
    }

}
