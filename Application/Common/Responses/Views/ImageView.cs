using Application.Common.Mappings;
using Application.Common.Supports;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;

namespace Application.Common.Responses.Views
{
    public class ImageView : IMapFrom<ImageEntity>
    {
        public string? Link { get; set; }
        public string Image { get; set; } = DefaultConstant.NoImage;
        public string ImageTitle { get; set; } = string.Empty;
        public string ImageAlt { get; set; } = string.Empty;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ImageEntity, ImageView>()
                .ForMember(d => d.Image, otp => otp.MapFrom(s => PathSupport.GetUploadImagePath(s.Name)))
                .ForMember(d => d.ImageTitle, otp => otp.MapFrom(s => s.Title))
                .ForMember(d => d.ImageAlt, otp => otp.MapFrom(s => s.Alt));
        }
    }
}
