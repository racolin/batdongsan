using Domain.Common;

namespace Domain.Entities
{
    public class ContentEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int? HomeImageId { get; set; }
        public virtual ImageEntity? HomeImage { get; set; }
        public int? BgHomeImageId { get; set; }
        public virtual ImageEntity? BgHomeImage { get; set; }
        public int? NewsImageId { get; set; }
        public virtual ImageEntity? NewsImage { get; set; }
        public int? ContactImageId { get; set; }
        public virtual ImageEntity? ContactImage { get; set; }
        public virtual ICollection<SliderImageEntity> ProjectSlider { get; set; } = new List<SliderImageEntity>();
        public string IntroduceSection { get; set; } = string.Empty;
        public string NewsMarketSection { get; set; } = string.Empty;
        public string NewsProjectSection { get; set; } = string.Empty;
        public string Status { get; set; } = StatusConstant.Draft;
    }
}
