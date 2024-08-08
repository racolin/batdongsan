using Domain.Common;

namespace Domain.Constants
{
    public record ProjectTypeConstant : IObtainableProperties
    {
        public const string Apartment = "apartment";
        public const string Ground = "ground";
        public const string ResortRealEstate = "resort-real-estate";
        public const string Villa = "villa";

        public static List<string> GetAllProperties()
        {
            return new List<string> { Apartment, Ground, ResortRealEstate, Villa };
        }
    }
    public record ProjectStateConstant : IObtainableProperties
    {
        public const string Implementing = "implementing";
        public const string Implemented = "implemented";

        public static List<string> GetAllProperties()
        {
            return new List<string> { Implementing, Implemented };
        }
    }
}