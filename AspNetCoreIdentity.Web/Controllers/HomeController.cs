using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using AspNetCoreIdentity.Web.Extensions;
using AspNetCoreIdentity.Web.Services;
using AspNetCoreIdentity.Web.ViewModels;

namespace AspNetCoreIdentity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> singInManager;
        private readonly IEmailService emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> singInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            this.singInManager = singInManager;
            this.emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult SingUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SingUp(SingUpViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityResult = await _userManager.CreateAsync(new() { UserName = request.UserName, Email = request.Email, PhoneNumber = request.Phone, },
                request.PasswordConfirm
                );

            if (identityResult.Succeeded)
            {
                TempData["SuccessMesssage"] = "Üyelik kayıt işlemi başarı ile gerçekleşmiştir";
                return RedirectToAction(nameof(HomeController.SingUp));
            }
            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
          
            

                return View();
        }
        public IActionResult SingIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SingIn(SingInViewModel request,string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");
            var hasUser=await _userManager.FindByEmailAsync(request.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty,"Email veya şifre yanlış");
            }
            var result = await singInManager.PasswordSignInAsync(hasUser,request.Password,request.RememberMe,true);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 daki ka boyunca kullanıcınız kitlenmiştir." });
                return View();
            }

            ModelState.AddModelErrorList(new List<string>() { "Email veya şifre yanlış." });
            ModelState.AddModelErrorList(new List<string>() { $"Kalan giriş hakkınız={3-await _userManager.GetAccessFailedCountAsync(hasUser)}" });
            return View();
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel request)
        {
            //https://localhost:7287?userId=1234$tok=qaadadsad

            var hasUser = await _userManager.FindByEmailAsync(request.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email adreisne ait bir kullanıc bulunamdı.");
                return View();
            }
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);
            var passwordResetLink = Url.Action("ResetPassword", "Home", new {userId=hasUser.Id,token=passwordResetToken},HttpContext.Request.Scheme);

            await emailService.SendResetPasswordEmail(passwordResetLink,hasUser.Email);



            TempData["SuccessMesssage"] = "Şifre yenileme linki email adresiniez gönderilmiştir.";
            return RedirectToAction(nameof(ForgotPassword));
        }

        public IActionResult ResetPassword(string userId,string token)
        {
           TempData["userId"] =userId; 
            TempData["token"] =token;
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId ==null || token==null)
            {
                throw new Exception("Bir hata meydana geldi");

            }
            var hasUser = await _userManager.FindByIdAsync(userId.ToString());

            if (hasUser == null) 
            {

                ModelState.AddModelError(string.Empty, "Kullanııc BUlunamadı");
            }

            IdentityResult result=await _userManager.ResetPasswordAsync(hasUser,token.ToString(),request.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMesssage"] = "Şifreniz yenilenmiştir.";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x=>x.Description).ToList());
                return View();
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}