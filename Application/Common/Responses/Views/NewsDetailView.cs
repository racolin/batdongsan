namespace Application.Common.Responses.Views
{
    public class NewsDetailView
    {
        public NewsView News { get; set; } = new NewsView();
        //public List<NewsView> RelativedNews { get; set; } = new List<NewsView>();
        public int HintType { get; set; }
        public string? HintLeftName { get; set; }
        public string? HintLeftUd { get; set; }
        public string? HintRightName { get; set; }
        public string? HintRightUd { get; set; }
    }

    public enum NewsHintType
    {
        Left = 1, Right = 2, Both = 3,
    }
}
