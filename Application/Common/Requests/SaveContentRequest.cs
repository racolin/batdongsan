using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Requests;

public class SaveContentRequest
{
    public bool IsUpdateImagePages { get; set; } = false;
    public bool IsUpdateSlider { get; set; } = false;
    public bool IsUpdateIntroduce { get; set; } = false;
    public List<ImagePageShortRequest> ImagePages { get; set; } = new List<ImagePageShortRequest>();
    public SliderShortRequest Slider { get; set; } = new SliderShortRequest();
    public SectionShortRequest Introduce { get; set; } = new SectionShortRequest();
    public List<SectionShortRequest> News { get; set; } = new List<SectionShortRequest>();
}

public class SectionShortRequest
{
    public int Id { get; set; }
    public int Position { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class SliderShortRequest
{
    public int Id { get; set; }
    public int Position { get; set; }
    public List<SliderImageShortRequest> Items { get; set; } = new List<SliderImageShortRequest>();
}

public class ImagePageShortRequest
{
    public int Id { get; set; }
    public int Position { get; set; }
    public int ImageId { get; set; }
}

public class SliderImageShortRequest : IMapFrom<SliderImageEntity>
{
    public int? Id { get; set; }
    public int Order { get; set; }
    public int ImageId { get; set; }
    public string? Link { get; set; }
}