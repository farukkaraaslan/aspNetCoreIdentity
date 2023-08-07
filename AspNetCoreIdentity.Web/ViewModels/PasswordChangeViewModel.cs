using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModels;

public class PasswordChangeViewModel
{

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Eski Parola Alanı boş bırakılamaz")]
    [Display(Name = "Eski Parola:")]
    public string PasswordOld { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Yeni Parola Alanı boş bırakılamaz")]
    [Display(Name = "Yeni Parola:")]
    public string PasswordNew { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(PasswordNewConfirm), ErrorMessage = "Paorlalar uyusmuyor")]
    [Required(ErrorMessage = "Parola Tekrar Alanı boş bırakılamaz")]
    [Display(Name = "Yeni Parola Tekrar:")]
    public string PasswordNewConfirm { get; set; }
}
