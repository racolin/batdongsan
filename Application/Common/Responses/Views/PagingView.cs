namespace Application.Common.Responses.Views
{
    public class PagingView
    {
        public int Current { get; set; } = 1;
        public int Max { get; set; } = 0;
        public List<NewsView> NewsList { get; set; } = new List<NewsView>();
    }
}
