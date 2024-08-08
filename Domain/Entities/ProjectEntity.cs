using Domain.Common;

namespace Domain.Entities
{
    public class ProjectEntity : BaseEntity
    {
        public string Ud { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Type { get; set; } = ProjectTypeConstant.Apartment;
        public string Content { get; set; } = string.Empty;
        public string State { get; set; } = ProjectStateConstant.Implementing;
        public string Status { get; set; } = StatusConstant.Draft;
        public int? Order { get; set; }
        public bool IsHighlight { get; set; } = false;
        public int? ImageId { get; set; }
        public virtual ImageEntity? Image { get; set; }
    }
}
