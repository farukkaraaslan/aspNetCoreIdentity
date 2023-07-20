using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentity.Web.Models;

//string property bizim için kullanıcı tablosundaki id guid olarak ayarlar.Burada int değeride verilebilir
public class AppDbContext :IdentityDbContext<AppUser,AppRole,string>
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        

    }
}
