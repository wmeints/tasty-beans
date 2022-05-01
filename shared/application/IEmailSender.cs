namespace TastyBeans.Shared.Application;

public interface IEmailSender
{
    Task SendEmailAsync(string emailAddress, string subject, string bodyHtml);
}