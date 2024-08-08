using Domain.Common;

namespace Domain.Entities
{
    public class SliderImageEntity : BaseEntity
    {
        public int Order { get; set; }
        public int ImageId { get; set; }
        public string? Link { get; set; }
        public virtual ImageEntity Image { get; set; }
    }
}
