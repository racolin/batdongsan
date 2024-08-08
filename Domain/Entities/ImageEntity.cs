using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class ImageEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Link { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;
        public int Type { get; set; } = (int)ImageTypeEnum.OnlyFull;
    }
}
