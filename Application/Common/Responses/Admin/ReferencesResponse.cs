namespace Application.Common.Responses.Admin;
public class ReferencesResponse
{
    public string Ud { get; set; } = string.Empty;
    public string UdName { get; set; } = string.Empty;
    public List<ReferenceResponse> References { get; set; } = new List<ReferenceResponse>();
}

public class ReferenceResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
