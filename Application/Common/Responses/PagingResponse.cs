namespace Application.Common.Responses
{
    public class PagingResponse<T>
    {
        public List<T> List { get; set; } = new List<T>();
        public int Current { get; set; }
        public int PerPage { get; set; }
        public int Max { get; set; }
        public bool CanPrevious => Current > 1;
        public bool CanNext => Current < Max;
        public static PagingResponse<T> Success(List<T> list, int current, int max, int perPage)
        {
            return new PagingResponse<T>
            {
                List = list,
                Current = current,
                Max = max,
                PerPage = perPage,
            };
        }
    }
}
