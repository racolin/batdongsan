using Domain.Common;

namespace Domain.Entities
{
    public class RegisterMailEntity : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string State { get; set; } = RegisterMailStateConstant.Sent;
    }
}
