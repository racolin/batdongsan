using Application.Common.Mappings;
using Application.Common.Supports;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;

namespace Application.Common.Requests;

public class SaveContentRequest : IMapFrom<ContentEntity>
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int HomeImageId { get; set; }
    public int BgHomeImageId { get; set; }
    public int NewsImageId { get; set; }
    public int ContactImageId { get; set; }
    public bool IsUpdateIntroduceSection { get; set; } = false;
    public string IntroduceSection { get; set; } = string.Empty;
    public string NewsMarketSection { get; set; } = string.Empty;
    public string NewsProjectSection { get; set; } = string.Empty;
    public bool IsUpdateProjectSlider { get; set; } = false;
    public List<SliderImageShortRequest> ProjectSlider { get; set; } = new List<SliderImageShortRequest>();
    public string Status { get; set; } = StatusConstant.Draft;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SaveContentRequest, ContentEntity>()
            .ForMember(d => d.ProjectSlider, otp => otp.MapFrom(s => s.ProjectSlider));
    }

}

public class SliderImageShortRequest : IMapFrom<SliderImageEntity>
{
    public int? Id { get; set; }
    public int Order { get; set; }
    public int ImageId { get; set; }
    public string? Link { get; set; }
}