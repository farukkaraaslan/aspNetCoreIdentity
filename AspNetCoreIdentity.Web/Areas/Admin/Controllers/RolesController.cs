using AspNetCoreIdentity.Web.Areas.Admin.Models;
using AspNetCoreIdentity.Web.Extensions;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentity.Web.Areas.Admin.Controllers;

[Authorize(Roles = "admin")]
[Area("Admin")]
public class RolesController : Controller
{
    private readonly UserManager<AppUser> userManager;
    private readonly RoleManager<AppRole> roleManager;

    public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }
    [Authorize(Roles = "role-action")]
    public async Task< IActionResult> Index()
    {
        var roles = await roleManager.Roles.Select(x => new RoleViewModel()
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync();

        return View(roles);
    }


    [Authorize(Roles ="role-action")]
    public IActionResult RoleCreate()
    {
        return View();
    }

      [Authorize(Roles ="role-action")]
    [HttpPost]
    public async Task<IActionResult> RoleCreate(RoleCreateViewModel request)
    {
        var result = await roleManager.CreateAsync(new AppRole() { Name = request.Name });
        if (!result.Succeeded)
        {
            ModelState.AddModelErrorList(result.Errors);
            return View();
        }
        TempData["SuccessMesssage"] = "Role başarı ile oluşturuldu";
        return RedirectToAction(nameof(RolesController.Index));
    }
    [Authorize(Roles = "role-action")]
    public async Task<IActionResult> RoleUpdate(string id)
    {
        var roleToUpdate = await roleManager.FindByIdAsync(id);

        if (roleToUpdate == null)
        {
            throw new Exception("Güncellenecek rol bulunamadı");
        }
        return View(new RoleUpdateViewModel() { Id=roleToUpdate.Id,Name=roleToUpdate.Name});
    }
    [HttpPost]
    [Authorize(Roles = "role-action")]
    public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel request)
    {
        var roleToUpdate= await roleManager.FindByIdAsync(request.Id);

        if (roleToUpdate == null)
        {
            throw new Exception("Güncellenecek rol bulunamadı");
        }
        roleToUpdate.Name=request.Name;

        await roleManager.UpdateAsync(roleToUpdate);
        TempData["SuccessMesssage"] = "Role başarı ile güncellendi";

        return View();
    }

    [Authorize(Roles = "role-action")]
    public async Task<IActionResult> RoleDelete(string id)
    {
        var roleToDelete= await roleManager.FindByIdAsync(id); 
        if (roleToDelete == null)
        {
            throw new Exception("silinecek role bulunamadı");
        }

        var result = await roleManager.DeleteAsync(roleToDelete);
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.Select(x => x.Description).First());
        }
        TempData["SuccessMesssage"] = "Role başarı ile silindi";
        return RedirectToAction("Index");
    }
    [Authorize(Roles = "role-action")]
    public async Task<IActionResult> AssingRoleTouser(string id)
    {
        var currentUser = await userManager.FindByIdAsync(id);
        ViewBag.userId = id;
        var roles = await roleManager.Roles.ToListAsync();
        var roleViewModelList= new List<AssingRoleToUserViewModel>();

        var userRoles= await userManager.GetRolesAsync(currentUser);
        foreach (var role in roles)
        {
            var assingRoleTOUserViewModel=new AssingRoleToUserViewModel()
            {
                Id= role.Id,
                Name=role.Name
            };

            if (userRoles.Contains(role.Name))
            {
                assingRoleTOUserViewModel.Exist = true;

            }
            roleViewModelList.Add(assingRoleTOUserViewModel);
        }
        return View(roleViewModelList);
    }

    [HttpPost]
    [Authorize(Roles = "role-action")]
    public async Task<IActionResult> AssingRoleTouser(string userId,List<AssingRoleToUserViewModel> requestList)
    {
        var userToAssingRoles= await userManager.FindByIdAsync(userId);
        foreach (var role in requestList)
        {
            if (role.Exist)
            {
                await userManager.AddToRoleAsync(userToAssingRoles,role.Name);
            }
            else
            {
                await userManager.RemoveFromRoleAsync(userToAssingRoles,role.Name);
            }
        }

       return RedirectToAction(nameof(HomeController.UserList),"Home");
    }
}
