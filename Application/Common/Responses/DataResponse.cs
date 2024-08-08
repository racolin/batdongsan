namespace Application.Common.Responses;
public class DataResponse<T>
{
    public T Data { get; set; }
    public Message Message { get; set; } = new();
    public static DataResponse<T> Success(T data)
    {
        return new DataResponse<T>
        {
            Data = data,
            Message = new Message
            {
                Type = MessageType.Success,
            }
        };
    }
    public static DataResponse<T> Warning(T data, string message, List<string> detail)
    {
        return new DataResponse<T>
        {
            Message = new Message()
            {
                Type = MessageType.Warning,
                Name = message,
                Detail = detail,
            }
        };
    }
    public static DataResponse<T> Warning(T data, string message)
    {
        return new DataResponse<T>
        {
            Message = new Message()
            {
                Type = MessageType.Warning,
                Name = message,
                Detail = [],
            }
        };
    }
    public static DataResponse<T> Error(string message, List<string> detail)
    {
        return new DataResponse<T>
        {
            Message = new Message()
            {
                Type = MessageType.Error,
                Name = message,
                Detail = detail,
            }
        };
    }
    public static DataResponse<T> Error(string message)
    {
        return new DataResponse<T>
        {
            Message = new Message()
            {
                Type = MessageType.Error,
                Name = message,
                Detail = [],
            }
        };
    }
}

public class Message
{
    public MessageType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Detail { get; set; } = new List<string>();
}

public enum MessageType
{
    Success,
    Warning,
    Error,
}