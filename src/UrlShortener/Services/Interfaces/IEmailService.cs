namespace UrlShortener.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(
        string receiverName,
        string receiverEmail,
        string subject,
        string body,
        bool html = true,
        CancellationToken token = default);
}