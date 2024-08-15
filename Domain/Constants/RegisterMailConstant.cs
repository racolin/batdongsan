using Domain.Common;

namespace Domain.Constants
{
    public record RegisterMailStateConstant : IObtainableProperties
    {
        public const string Sent = "sent";
        public const string Added = "added";
        public const string Expired = "expired";
        public const string Removed = "removed";

        public static List<string> GetAllProperties()
        {
            return new List<string> { Sent, Added, Expired, Removed };
        }
    }
}