using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using UrlShortener.WebApi.Models.Settings;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Services.Email;

public class SmtpEmailService : IEmailService
{
    private readonly IOptionsSnapshot<SmtpSettings> _smtpSettings;

    public SmtpEmailService(IOptionsSnapshot<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings;
    }

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
            message.From.Add(new MailboxAddress(_smtpSettings.Value.Name, _smtpSettings.Value.Email));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            var client = new SmtpClient();
            await client.ConnectAsync(_smtpSettings.Value.Host, _smtpSettings.Value.Port, cancellationToken: ct);
            await client.AuthenticateAsync(_smtpSettings.Value.Email, _smtpSettings.Value.Password, ct);
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
        string subject,
        string templateId,
        object templateData,
        CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}