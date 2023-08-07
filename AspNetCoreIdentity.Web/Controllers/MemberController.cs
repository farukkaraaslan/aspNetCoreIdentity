using AspNetCoreIdentity.Web.Extensions;
using AspNetCoreIdentity.Web.Models;
using AspNetCoreIdentity.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace AspNetCoreIdentity.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> singInManager;
        private readonly IFileProvider fileProvider;

        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> singInManager, IFileProvider fileProvider)
        {
            this.userManager = userManager;
            this.singInManager = singInManager;
            this.fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser =await userManager.FindByNameAsync(User.Identity.Name);
            var userViewModel = new UserViewModel
            {
                Email = currentUser.Email,
                PhoneNumber = currentUser.UserName,
                UserName = currentUser.UserName,
                PictureUrl=currentUser.Picture,
            };
            return View(userViewModel);
        }
        public async Task Logout()
        {
           await singInManager.SignOutAsync();
        }
        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();

            }

            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            var checkOldPassword = await userManager.CheckPasswordAsync(currentUser, request.PasswordOld);
            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski parola yanlış");
            }

            var resultChangePasword = await userManager.ChangePasswordAsync(currentUser, request.PasswordOld, request.PasswordNew);

            if (!resultChangePasword.Succeeded)
            {
                ModelState.AddModelErrorList(resultChangePasword.Errors.Select(x => x.Description).ToList());
                return View();
            }

            TempData["SuccessMesssage"] = "Parola başarıile değiştirdi.";
            await userManager.UpdateSecurityStampAsync(currentUser);
            await singInManager.SignOutAsync();
            await singInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true, false);

            return View();
        }


        public async Task<IActionResult> UserEdit() 
        {
            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));

            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                Gender = currentUser.Gender,
                BirtDate = currentUser.BirthDate,
                City = currentUser.City,

            };
            return View(userEditViewModel); 
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            currentUser.UserName=request.UserName;
            currentUser.Email=request.Email;
            currentUser.BirthDate=request.BirtDate; 
            currentUser.City=request.City;
            currentUser.PhoneNumber=request.Phone; 
            currentUser.Gender=request.Gender;
            if(request.Picture!=null && request.Picture.Length > 0)
            {
                var wwwrootFolder = fileProvider.GetDirectoryContents("wwwroot");
                var rondomFileName= $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";
                var newPicturePath=Path.Combine(wwwrootFolder.First(x=>x.Name=="UserPicture").PhysicalPath, rondomFileName);
                using var steram = new FileStream(newPicturePath, FileMode.Create);
                await request.Picture.CopyToAsync(steram);
                currentUser.Picture=rondomFileName;
            }

            var updateToUserResult= await userManager.UpdateAsync(currentUser);

            if (!updateToUserResult.Succeeded)
            {
                ModelState.AddModelErrorList(updateToUserResult.Errors);
                return View();
            }

            await userManager.UpdateSecurityStampAsync(currentUser);
            await singInManager.SignOutAsync();
            await singInManager.SignInAsync(currentUser, true);

            TempData["SuccessMesssage"] = "Üye bilgileri başarı ile değiştirildi.";
            return View();
        }


        public IActionResult AccsessDenied(string ReturnUrl)
        {
            string message =string.Empty;
            message = "Bu sayfayı görüntülemek için yetkiniz yoktur. Lütfen yöneticinize başvurunuz.";
            ViewBag.message=message;
            return View();
        }

        [HttpGet]
        public IActionResult Claims()
        {
            var userClaimList= User.Claims.Select(x=> new ClaimViewModel()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value,
            }).ToList();
            return View(userClaimList);
        }

        [Authorize(Policy ="AnkaraPolicy")]
        [HttpGet]
        public IActionResult AnkaraPage()
        {
            return View();
        }
    }
}
