using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.localization;

public class LocalizationIdentityErrorDescriber: IdentityErrorDescriber
{
    public override IdentityError DuplicateUserName(string userName)
    {
        return new ()
        {
            Code = "DuplicateUserName",
            Description = $"{userName} daha önce başka bir kullanıcı tarafından alınmıştır."
        };
    }
    public override IdentityError DuplicateEmail(string email)
    {
        return new()
        {
            Code = "DuplicateEmail",
            Description = $" {email} başka bir kullanıcı tarafından kullanılmaktadır."
        };
    }
}
