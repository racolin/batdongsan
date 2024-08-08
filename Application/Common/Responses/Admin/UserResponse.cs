using Domain.Enums;

namespace Application.Common.Responses.Admin;

public class UserResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int Gender { get; set; } = (int)GenderEnum.Male;
    public DateTime? DateOfBirth { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public List<string> Roles { get; set; }
}