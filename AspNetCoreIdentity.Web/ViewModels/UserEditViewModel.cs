using AspNetCoreIdentity.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModels;

public class UserEditViewModel
{
    [Required(ErrorMessage = "Kullanıcı Adı Alanı boş bırakılamaz")]
    [Display(Name = "Kullanıcı Adı:")]
    public string UserName { get; set; }

    [EmailAddress(ErrorMessage = "Email formatı uygun değil")]
    [Required(ErrorMessage = "Mail Alanı boş bırakılamaz")]
    [Display(Name = "Mail:")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Telefon Alanı boş bırakılamaz")]
    [Display(Name = "Telefon:")]
    public string Phone { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Doğum Tarihi:")]
    public DateTime? BirtDate { get; set; }

    [Display(Name = "Cinsiyet:")]
    public Gender? Gender { get; set; }

    [Display(Name = "Şehir:")]
    public string? City { get; set; }

    [Display(Name = "Resim:")]
    public IFormFile? Picture { get; set; }
}
