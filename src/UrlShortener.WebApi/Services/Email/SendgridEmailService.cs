using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using UrlShortener.WebApi.Models.EmailTemplates;
using UrlShortener.WebApi.Models.Settings;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Services.Email;

public class SendgridEmailService : IEmailService
{
    private readonly IOptions<SendGridSettings> _sendGridSettings;

    public SendgridEmailService(IOptions<SendGridSettings> sendGridSettings)
    {
        _sendGridSettings = sendGridSettings;
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
            var client = new SendGridClient(_sendGridSettings.Value.SecretKey);

            var from = new EmailAddress(_sendGridSettings.Value.Email, _sendGridSettings.Value.Name);
            var to = new EmailAddress(receiverEmail, receiverName);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, body);

            var response = await client.SendEmailAsync(msg, ct);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendTemplateEmailAsync(
        string receiverName,
        string receiverEmail,
        string templateId,
        BaseEmailTemplateData templateData,
        CancellationToken ct = default)
    {
        try
        {
            var client = new SendGridClient(_sendGridSettings.Value.SecretKey);

            var msg = new SendGridMessage();
            msg.SetFrom(_sendGridSettings.Value.Email, _sendGridSettings.Value.Name);
            msg.AddTo(receiverEmail, receiverName);

            msg.SetTemplateId(templateId);
            msg.SetTemplateData(templateData);

            var response = await client.SendEmailAsync(msg, ct);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}