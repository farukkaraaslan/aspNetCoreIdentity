using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.Areas.Admin.Models;

public class RoleUpdateViewModel
{
    public string Id { get; set; }
    [Required(ErrorMessage = "Role adı alanı boş bırakılamaz")]
    [Display(Name = "Role Adı:")]
    public string Name { get; set; }
}
