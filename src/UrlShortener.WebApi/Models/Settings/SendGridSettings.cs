namespace UrlShortener.WebApi.Models.Settings;

public class SendGridSettings
{
    public string Email { get; set; }
    
    public string Name { get; set; }

    public string SecretKey { get; set; }
    
    public string VerificationEmailTemplateId { get; set; }
    
    public string PasswordChangedAlertTemplateId { get; set; }

    public string PasswordRecoveryTemplateId { get; set; }
}