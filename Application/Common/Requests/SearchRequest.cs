using FluentValidation;

namespace Application.Common.Requests
{
    public class SearchRequest
    {
        public string? Value { get; set; }
        public string? ValueFilter1 { get; set; }
        public string? ValueFilter2 { get; set; }
        public int? CurrentPage { get; set; }
        public int? PerPage { get; set; }
        public int Start { 
            get { 
                if (CurrentPage == null || CurrentPage < 1) return 0;
                return (CurrentPage.Value - 1) * Length;
            } 
        }
        public int Length { 
            get { 
                return PerPage == null || PerPage < 1 ?  10 : PerPage.Value; 
            } 
        }
        public List<string> Status { get; set; } = new List<string>();
        public List<string> State { get; set; } = new List<string>();
        public List<bool> HighLight { get; set; } = new List<bool>();
        public List<bool> IsLock { get; set; } = new List<bool>();
        public List<string> Type { get; set; } = new List<string>();
        public string? StartDate { get; set; }
        public DateTime? StartDateTime
        {
            get
            {
                DateTime? start = null;
                if (StartDate != null)
                {
                    try
                    {
                        start = DateTime.ParseExact(StartDate, "dd-MM-yyyy", null);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return start;
            }
        }
        public string? EndDate { get; set; }
        public DateTime? EndDateTime
        {
            get
            {
                DateTime? end = null;
                if (EndDate != null)
                {
                    try
                    {
                        end = DateTime.ParseExact(EndDate, "dd-MM-yyyy", null);
                        end = end?.AddDays(1);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return end;
            }
        }
        public string? Order { get; set; }
        public bool? IsAsc {
            get
            {
                return Order == null ? null : "asc".Equals(Order.Split("-")[0].ToLower());
            }
        }
        public string? OrderType {
            get
            {
                return Order == null ? null : Order.Split("-")[1].ToLower();
            }
        }
    }

    public class NewSearchValidator : AbstractValidator<SearchRequest> 
    { 
        public NewSearchValidator() 
        {
            RuleFor(x => x.Start).GreaterThan(0).WithMessage("Giá trị bắt đầu phải lớn hơn 0");    
            RuleFor(x => x.Length).GreaterThan(0).WithMessage("Số phần tử phải lớn hơn 0");    
            RuleFor(x => x.Order).Must((x, order) =>
            {
                return order == null ? true : (order.Split("-").Length == 2);
            }).WithMessage("Order không đúng định dạng");    
        }
    }
}
