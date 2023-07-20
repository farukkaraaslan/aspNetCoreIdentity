using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModels;

public class ForgotPasswordViewModel
{
    [EmailAddress(ErrorMessage = "Email formatı uygun değil")]
    [Required(ErrorMessage = "Mail Alanı boş bırakılamaz")]
    [Display(Name = "Mail:")]
    public string Email { get; set; }

}
