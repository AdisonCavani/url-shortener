using Newtonsoft.Json;

namespace UrlShortener.WebApi.Models.EmailTemplates;

public class VerificationEmailTemplateData : BaseEmailTemplateData
{
    [JsonProperty("token")]
    public string Token { get; set; }
}