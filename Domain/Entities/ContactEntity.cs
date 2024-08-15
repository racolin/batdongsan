using Domain.Common;

namespace Domain.Entities
{
    public class ContactEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string State { get; set; } = ContactStateConstant.Sent;
    }
}
