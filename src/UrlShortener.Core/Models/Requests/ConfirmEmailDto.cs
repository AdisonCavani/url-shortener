namespace UrlShortener.Core.Models.Requests;

public class ConfirmEmailDto
{
    public string Email { get; init; }

    public string Token { get; init; }
}
