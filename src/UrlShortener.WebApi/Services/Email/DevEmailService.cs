using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using UrlShortener.WebApi.Models.EmailTemplates;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Services.Email;

public class DevEmailService : IEmailService
{
    public async Task<bool> SendEmailAsync(
        string receiverName,
        string receiverEmail,
        string subject,
        string body,
        CancellationToken ct = default)
    {
        try
        {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress("Papercut", "chat@papercut.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            var client = new SmtpClient();
            await client.ConnectAsync("localhost", cancellationToken: ct);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<bool> SendTemplateEmailAsync(
        string receiverName,
        string receiverEmail,
        string templateId,
        BaseEmailTemplateData templateData,
        CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}