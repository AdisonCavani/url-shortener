using Newtonsoft.Json;

namespace UrlShortener.WebApi.Models.EmailTemplates;

public class BaseEmailTemplateData
{
    [JsonProperty("subject")]
    public string Subject { get; set; }
    
    [JsonProperty("preheader")]
    public string Preheader { get; set; }
}