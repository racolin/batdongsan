using Domain.Common;

namespace Domain.Constants
{
    public record StatusConstant : IObtainableProperties
    {
        public const string Draft = "Draft";
        public const string Active = "Active";
        public const string InActive = "InActive";
        public const string Removed = "Removed";

        public static List<string> GetAllProperties()
        {
            return new List<string> { Draft, Active, InActive, Removed };
        }
    }
}