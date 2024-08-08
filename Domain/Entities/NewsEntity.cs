using Domain.Common;

namespace Domain.Entities
{
    public class NewsEntity : BaseEntity
    {
        public string Ud { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = NewsTypeConstant.Project;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Status { get; set; } = StatusConstant.Draft;
        public int? Order { get; set; }
        public bool IsHighlight { get; set; } = false;
        public int? ImageId { get; set; }
        public virtual ImageEntity? Image { get; set; }
    }
}
