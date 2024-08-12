namespace Application.Common.Responses.Views
{
    public class HomeIndexView
    {
        public ImageView BackgroundImage { get; set; } = new ImageView();
        public ImageView TopImage { get; set; } = new ImageView();
        public string Content { get; set; } = string.Empty;
        public string DescriptionNewsProject { get; set; } = string.Empty;
        public string DescriptionNewsMarket { get; set; } = string.Empty;
        public List<NewsView> NewsList { get; set; } = new List<NewsView>();
        public List<ProjectView> Projects { get; set; } = new List<ProjectView>();
    }
}
