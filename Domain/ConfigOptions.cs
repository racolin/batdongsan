namespace Domain
{
    public class ConfigOptions
    {
        public const string Name = "ConfigOptions";

        public int DefaultPageSize { get; set; }
        public SmtpConfig Smtp { get; set; }
    }
    public class SmtpConfig
    {
        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public bool DefaultCredential { get; set; }
    }
    public class JwtConfigOptions
    {
        public const string Name = "JWT";

        public string Secret { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public int TokenValidityInMinutes { get; set; }
        public int RefreshTokenValidityInDays { get; set; }
    }
    
    public class RecaptchaConfigOptions
    {
        public const string Name = "Recaptcha";

        public RecaptchaV3ConfigOptions V3 { get; set; }
    }

    public class RecaptchaV3ConfigOptions
    {

        public string SecretKey { get; set; }
        public string SiteKey { get; set; }
        public string SiteVerify { get; set; }
    }

}