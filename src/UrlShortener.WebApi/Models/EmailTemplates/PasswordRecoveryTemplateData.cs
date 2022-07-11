using Newtonsoft.Json;

namespace UrlShortener.WebApi.Models.EmailTemplates;

public class PasswordRecoveryTemplateData : BaseEmailTemplateData
{
    [JsonProperty("token")]
    public string Token { get; set; }
}