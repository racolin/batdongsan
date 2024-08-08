using Domain.Common;

namespace Domain.Entities
{
    public class ImagePageEntity : BaseEntity
    {
        public int ImageId { get; set; }
        public virtual ImageEntity Image { get; set; }
        public int? Position { get; set; }
    }
}
