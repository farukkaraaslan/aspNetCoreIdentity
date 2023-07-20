using AspNetCoreIdentity.Web.Areas.Admin.Models;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentity.Web.Areas.Admin.Controllers;
[Area("Admin")]
public class HomeController : Controller
{
    private readonly UserManager<AppUser> userManager;

    public HomeController(UserManager<AppUser> userManager)
    {
        this.userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> UserList()
    {
        var userList = await userManager.Users.ToListAsync();
        var userViewModelList = userList.Select(x => new UserViewModel()
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.UserName,
        });
        return View(userViewModelList.ToList());
    }
}
