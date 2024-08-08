using Domain.Common;

namespace Domain.Entities
{
    public class SectionEntity : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public int? Position { get; set; }
    }
}
