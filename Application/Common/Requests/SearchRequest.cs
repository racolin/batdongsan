using FluentValidation;

namespace Application.Common.Requests
{
    public class SearchRequest
    {
        public string? Value { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<string> Status { get; set; } = new List<string>();
        public string? Order { get; set; }
        public bool? IsAsc { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ParentId { get; set; }
        public string? ValueFilter1 { get; set; }
        public string? ValueFilter2 { get; set; }
        public List<string> ValuesFilter1 { get; set; } = new List<string>();
        public List<string> ValuesFilter2 { get; set; } = new List<string>();
    }

    public class SearchValidator : AbstractValidator<SearchRequest> 
    { 
        public SearchValidator() 
        {
            RuleFor(x => x.Start).GreaterThan(0).WithMessage("Giá trị bắt đầu phải lớn hơn 0");    
            RuleFor(x => x.Length).GreaterThan(0).WithMessage("Số phần tử phải lớn hơn 0");    
        }
    }
}
