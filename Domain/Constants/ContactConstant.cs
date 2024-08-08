using Domain.Common;

namespace Domain.Constants
{
    public record ContactStateConstant : IObtainableProperties
    {
        public const string Sent = "sent";
        public const string Processed = "processed";
        public const string Refused = "refused";
        public const string Removed = "removed";

        public static List<string> GetAllProperties()
        {
            return new List<string>() { Sent, Processed, Refused, Removed };
        }
    }
}