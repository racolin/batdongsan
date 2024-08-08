namespace Application.Common.Responses.Views
{
    public class NewsIndexView
    {
        public ImageView TopImage { get; set; } = new ImageView();
        public PagingView Paging { get; set; } = new PagingView();
    }
}
