using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModels;

public class SingUpViewModel
{
    public SingUpViewModel()
    {

    }
    public SingUpViewModel(string username, string email, string phone, string password)
    {
        UserName = username;
        Email = email;
        Phone = phone;
        Password = password;
    }

    [Required(ErrorMessage ="Kullanıcı Adı Alanı boş bırakılamaz")]
    [Display(Name ="Kuallanıcı Adı:")]
    public string UserName { get; set; }

    [EmailAddress(ErrorMessage ="Email formatı uygun değil")]
    [Required(ErrorMessage = "Mail Alanı boş bırakılamaz")]
    [Display(Name = "Mail:")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Telefon Alanı boş bırakılamaz")]
    [Display(Name = "Telefon:")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Parola Alanı boş bırakılamaz")]
    [Display(Name = "Parola:")]
    public string Password  { get; set; }

    [Compare(nameof(Password),ErrorMessage ="Paorlalar uyusmuyor")]
    [Required(ErrorMessage = "Parola Tekrar Alanı boş bırakılamaz")]
    [Display(Name = "Parola Tekrar:")]
    public string PasswordConfirm { get; set; }
}
