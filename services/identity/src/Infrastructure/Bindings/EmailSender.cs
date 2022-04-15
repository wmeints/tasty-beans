using System.Net.Mail;
using Dapr.Client;
using RecommendCoffee.Identity.Application.Common;

namespace RecommendCoffee.Identity.Infrastructure.Bindings;

public class EmailSender : IEmailSender
{
    private readonly DaprClient _daprClient;

    public EmailSender(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task SendEmailAsync(string emailAddress, string subject, string bodyHtml)
    {
        var metadata = new Dictionary<string, string>
        {
            { "emailTo", emailAddress },
            { "subject", subject },
            { "emailFrom", "noreply@recommend.coffee" }
        };

        // await _daprClient.InvokeBindingAsync("smtp", "create", bodyHtml, metadata);

        var client = new SmtpClient("emailserver", 25);
        var message = new MailMessage("noreply@recommend.coffee", emailAddress, subject, bodyHtml)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }
}