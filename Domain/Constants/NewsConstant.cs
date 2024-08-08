using Domain.Common;
using System;

namespace Domain.Constants
{
    public record NewsTypeConstant : IObtainableProperties
    {
        public const string Project = "project";
        public const string Market = "market";

        public static List<string> GetAllProperties()
        {
            return new List<string> { Project, Market };
        }
    }
}