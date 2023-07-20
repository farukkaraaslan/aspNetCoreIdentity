using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> singInManager;

        public MemberController(SignInManager<AppUser> singInManager)
        {
            this.singInManager = singInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task Logout()
        {
           await singInManager.SignOutAsync();
        }
    }
}
