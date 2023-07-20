namespace AspNetCoreIdentity.Web.Services;

public interface IEmailService
{

    Task SendResetPasswordEmail(string resetEmailLink, string sendToEmail);
}
