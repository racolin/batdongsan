using Application.Common.Mappings;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Requests;

public class SaveImageRequest : IMapFrom<ImageEntity>
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty; 
    public string? Link { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Alt { get; set; } = string.Empty;
    public int Type { get; set; }
    public IFormFile? Large { get; set; }
    public IFormFile? Small { get; set; }
}

public class SaveImageValidator : AbstractValidator<SaveImageRequest>
{
    public SaveImageValidator()
    {
        RuleFor(x => x.Id)
        .Must((x, id) => {
            return !(string.IsNullOrEmpty(x.Name) || string.IsNullOrEmpty(x.Title) || string.IsNullOrEmpty(x.Alt));
        }).WithMessage("Không được để trống dữ liệu bắt buộc!")
        .Must((x, id) => {
            if (id == null)
            {
                return !(string.IsNullOrEmpty(x.Name) 
                        || string.IsNullOrEmpty(x.Title) 
                        || string.IsNullOrEmpty(x.Alt)
                        || x.Large == null
                        || (x.Type == (int)ImageTypeEnum.HasThumb && x.Small == null)
                );
            }
            return true;
        }).WithMessage("Không để trống những thuộc tính bắt buộc khi tạo mới!");
    }
}