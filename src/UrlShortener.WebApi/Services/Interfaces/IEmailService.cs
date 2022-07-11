using UrlShortener.WebApi.Models.EmailTemplates;

namespace UrlShortener.WebApi.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(
        string receiverName,
        string receiverEmail,
        string subject,
        string body,
        CancellationToken ct = default);

    Task<bool> SendTemplateEmailAsync(
        string receiverName,
        string receiverEmail,
        string templateId,
        BaseEmailTemplateData templateData,
        CancellationToken ct = default);
}