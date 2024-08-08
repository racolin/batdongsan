using Application.Common.Mappings;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;

namespace Application.Common.Requests;

public class SaveProjectRequest : IMapFrom<ProjectEntity>
{
    public int? Id { get; set; }
    public string Ud { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = ProjectTypeConstant.Apartment;
    public string Address { get; set; } = string.Empty;
    public string State { get; set; } = ProjectStateConstant.Implementing;
    public string Content { get; set; } = string.Empty;
    public bool IsUpdateContent { get; set; } = true;
    public string Status { get; set; } = StatusConstant.Draft;
    public int? Order { get; set; }
    public bool IsHighlight { get; set; } = false;
    public int? ImageId { get; set; }
}

public class SaveProjectValidator : AbstractValidator<SaveProjectRequest>
{
    public SaveProjectValidator()
    {
        RuleFor(x => x.Id)
        .Must((x, id) =>
        {
            return !(string.IsNullOrEmpty(x.Name) || string.IsNullOrEmpty(x.Ud) || string.IsNullOrEmpty(x.Address));
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

        RuleFor(x => x.State)
        .Must((x, state) =>
        {
            switch (state)
            {
                case ProjectStateConstant.Implemented:
                case ProjectStateConstant.Implementing: return true;
                default: return false;
            }
        }).WithMessage("Tình trạng không xác định!");

        RuleFor(x => x.Type)
            .Must((x, type) => {
                switch (type)
                {
                    case ProjectTypeConstant.Apartment:
                    case ProjectTypeConstant.Ground:
                    case ProjectTypeConstant.ResortRealEstate:
                    case ProjectTypeConstant.Villa: return true;
                    default: return false;
                }
            }).WithMessage("Loại dự án không xác định!");
    }
}