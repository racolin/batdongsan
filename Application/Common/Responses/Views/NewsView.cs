using Application.Common.Mappings;
using Application.Common.Supports;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;

namespace Application.Common.Responses.Views
{
    public class NewsView : IMapFrom<NewsEntity>
    {
        public string Ud { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string TypeUd { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Image { get; set; } = DefaultConstant.NoImage;
        public string ImageTitle { get; set; } = string.Empty;
        public string ImageAlt { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewsEntity, NewsView>()
                .ForMember(d => d.TypeUd, otp => otp.MapFrom(s => s.Type))
                .ForMember(d => d.TypeName, otp => otp.MapFrom(s => NameSupport.GetNewsTypeName(s.Type)))
                .ForMember(d => d.Image, otp => otp.MapFrom(s => s.Image != null ? PathSupport.GetUploadImagePath(s.Image!.Name) : DefaultConstant.NoImage))
                .ForMember(d => d.ImageTitle, otp => otp.MapFrom(s => s.Image == null ? string.Empty : s.Image!.Title))
                .ForMember(d => d.ImageAlt, otp => otp.MapFrom(s => s.Image == null ? string.Empty : s.Image!.Alt))
                .ForMember(d => d.CreatedDate, otp => otp.MapFrom(s => s.PublishDate == null ? s.CreatedDate : s.PublishDate));
        }
    }
}
