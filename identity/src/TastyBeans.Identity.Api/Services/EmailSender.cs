using System.Net.Mail;

namespace TastyBeans.Identity.Api.Services;

public class EmailSender
{
    private readonly SmtpClient _smtpClient;
    
    public EmailSender(string hostName, int port)
    {
        _smtpClient = new SmtpClient(hostName, port);
    }
    
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MailMessage(
            "noreply@tastybeans.app", email, subject, message);

        await _smtpClient.SendMailAsync(emailMessage);
    }
}