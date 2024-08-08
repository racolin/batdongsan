using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class User : IdentityUser<int>
{
    public string Name { get; set; } = string.Empty;
    public int Gender { get; set; } = (int)GenderEnum.Male;
    public DateTime? DateOfBirth { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
}
