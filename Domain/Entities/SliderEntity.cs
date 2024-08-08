using Domain.Common;

namespace Domain.Entities
{
    public class SliderEntity : BaseEntity
    {
        public int? Position { get; set; }
        public virtual ICollection<SliderImageEntity> Items { get; set; }
    }
}
