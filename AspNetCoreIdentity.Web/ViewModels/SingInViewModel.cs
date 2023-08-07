using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModels;

public class SingInViewModel
{

    public SingInViewModel()
    {
        
    }

    public SingInViewModel(string email, string password)
    {
        Email = email;
        Password = password;
    }

    [EmailAddress(ErrorMessage = "Email formatı uygun değil")]
    [Required(ErrorMessage = "Mail Alanı boş bırakılamaz")]
    [Display(Name = "Mail:")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Parola Alanı boş bırakılamaz")]
    [Display(Name = "Parola:")]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}
