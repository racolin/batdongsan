namespace Application.Common.Requests
{
    public class AnswerRequest<T>
    {
        public string Id { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public T Date { get; set; }
    }
}
