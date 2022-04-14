namespace RecommendCoffee.Identity.Application.Common;

public interface IEmailSender
{
    Task SendEmailAsync(string emailAddress, string subject, string bodyHtml);
}