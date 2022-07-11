using Newtonsoft.Json;

namespace UrlShortener.WebApi.Models.EmailTemplates;

public class PasswordChangedAlertTemplateData : BaseEmailTemplateData
{
    [JsonProperty("firstName")]
    public string FirstName { get; set; }
    
    [JsonProperty("lastName")]
    public string LastName { get; set; }
}