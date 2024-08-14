using Domain.Common;

namespace Domain.Entities
{
    public class ConfigurationEntity : BaseEntity
    {
        public string SendEmail { get; set; }
        public string SendEmailPassword { get; set; }
        public string ReceiveEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactZaloPhone { get; set; }
        public string SystemPhone { get; set; }
        public string SystemEmail { get; set; }
        public string SystemZaloLink { get; set; }
        public string SystemYoutubeLink { get; set; }
        public string SystemFacebookLink { get; set; }
    }
}
