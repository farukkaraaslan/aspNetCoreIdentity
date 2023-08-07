using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace AspNetCoreIdentity.Web.TagHelpers
{
    public class UserRoleNamesTagHelper :TagHelper
    {
        public string UserId { get; set; }
        private readonly UserManager<AppUser> userManager;

        public UserRoleNamesTagHelper(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await userManager.FindByIdAsync(UserId);
            var userRoles =await userManager.GetRolesAsync(user);

            var stringBuilder = new StringBuilder();
            userRoles.ToList().ForEach(x=>
            {
                stringBuilder.Append(@$"<span class='badge bg-secondary mx-1'>{x.ToLower()}</span>");
            });

            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}
