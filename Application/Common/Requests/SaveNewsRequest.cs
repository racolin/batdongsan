using Application.Common.Mappings;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;

namespace Application.Common.Requests;

public class SaveNewsRequest : IMapFrom<NewsEntity>
{
    public int? Id { get; set; }
    public string Ud { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = NewsTypeConstant.Project;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsUpdateContent { get; set; } = true;
    public string Status { get; set; } = StatusConstant.Draft;
    public int? Order { get; set; }
    public bool IsHighlight { get; set; } = false;
    public int? ImageId { get; set; }
    public string? PubDate { get; set; }
}

public class SaveNewsValidator : AbstractValidator<SaveNewsRequest>
{
    public SaveNewsValidator()
    {
        RuleFor(x => x.Id)
        .Must((x, id) =>
        {
            return !(string.IsNullOrEmpty(x.Name) || string.IsNullOrEmpty(x.Ud) || string.IsNullOrEmpty(x.Description));
        }).WithMessage("Không được để trống dữ liệu bắt buộc!");

        RuleFor(x => x.Status)
        .Must((x, status) =>
        {
            switch (status)
            {
                case StatusConstant.Draft:
                case StatusConstant.Active:
                case StatusConstant.InActive:
                case StatusConstant.Removed: return true;
                default: return false;
            }
        }).WithMessage("Trạng thái không xác định!");

        RuleFor(x => x.Type)
            .Must((x, type) => {
                switch (type)
                {
                    case NewsTypeConstant.Project:
                    case NewsTypeConstant.Market: return true;
                    default: return false;
                }
            }).WithMessage("Loại tin tức không xác định!");
    }
}