using AspNetCoreIdentity.Web.OptionsModel;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentity.Web.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings emailSettings;

    public EmailService(IOptions<EmailSettings> options)
    {
        emailSettings = options.Value;
    }

    public async Task SendResetPasswordEmail(string resetEmailLink, string sendToEmail)
    {
        var smtpClient = new SmtpClient();

        smtpClient.Host=emailSettings.Host;
        smtpClient.DeliveryMethod=SmtpDeliveryMethod.Network;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.EnableSsl = true;
        smtpClient.Port = 587;
        smtpClient.Credentials=new NetworkCredential(emailSettings.Email,emailSettings.Password);


        var mailMessage = new MailMessage();
        mailMessage.From= new MailAddress(emailSettings.Email);
        mailMessage.To.Add(sendToEmail);

        mailMessage.Subject = "Localhost Şifre Sıfırlama Linki";

        mailMessage.Body = @$"
                  <h4>Şifrenizi yenilemek için aşağıdaki linkte tıklayınız.</h4>
                  <p><a href='{resetEmailLink}'>şifre yenileme link</a></p>";
        Console.WriteLine(mailMessage.Body);
        mailMessage.IsBodyHtml = true;

        await smtpClient.SendMailAsync(mailMessage);
    }
}
