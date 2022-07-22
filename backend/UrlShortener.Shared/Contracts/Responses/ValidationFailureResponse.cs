namespace UrlShortener.Shared.Contracts.Responses;

public class ValidationFailureResponse
{
    public IEnumerable<string> Errors { get; init; } = Enumerable.Empty<string>();
}
