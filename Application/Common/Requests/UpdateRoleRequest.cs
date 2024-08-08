namespace Application.Common.Requests
{
    public class UpdateRoleRequest
    {
        public int UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
