namespace Domain.Common
{
    public interface IObtainableProperties
    {
        public static List<string> GetAllProperties()
        {
            return new List<string>();
        }
        public static bool IsExistProperty(string property)
        {
            return GetAllProperties().Contains(property);
        }
    }
}
