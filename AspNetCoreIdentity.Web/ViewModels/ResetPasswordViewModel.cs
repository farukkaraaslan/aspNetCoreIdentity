using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModels;

public class ResetPasswordViewModel
{
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Parola Alanı boş bırakılamaz")]
    [Display(Name = "Parola:")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Paorlalar uyusmuyor")]
    [Required(ErrorMessage = "Parola Tekrar Alanı boş bırakılamaz")]
    [Display(Name = "Parola Tekrar:")]
    public string PasswordConfirm { get; set; }
}
