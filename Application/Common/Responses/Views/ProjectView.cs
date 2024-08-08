using Application.Common.Mappings;
using Application.Common.Supports;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;

namespace Application.Common.Responses.Views
{
    public class ProjectView : IMapFrom<ProjectEntity>
    {
        public string Ud { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string TypeUd { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string StateName { get; set; } = string.Empty;
        public string Image { get; set; } = DefaultConstant.NoImage;
        public string ImageTitle { get; set; } = string.Empty;
        public string ImageAlt { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProjectEntity, ProjectView>()
                .ForMember(d => d.TypeUd, otp => otp.MapFrom(s => s.Type))
                .ForMember(d => d.TypeName, otp => otp.MapFrom(s => NameSupport.GetProjectTypeName(s.Type)))
                .ForMember(d => d.StateName, otp => otp.MapFrom(s => NameSupport.GetProjectStateName(s.State)))
                .ForMember(d => d.Image, otp => otp.MapFrom(s => s.Image != null ? PathSupport.GetUploadImagePath(s.Image!.Name) : DefaultConstant.NoImage))
                .ForMember(d => d.ImageTitle, otp => otp.MapFrom(s => s.Image == null ? string.Empty : s.Image!.Title))
                .ForMember(d => d.ImageAlt, otp => otp.MapFrom(s => s.Image == null ? string.Empty : s.Image!.Alt));
        }
    }
}
